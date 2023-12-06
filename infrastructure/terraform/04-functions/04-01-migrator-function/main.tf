locals {
  name_prefix = "${var.environment}-zev"
  files_to_exclude_from_package = setunion(
    fileset("../../../../src", "**/{bin,obj}/**"),
    [".idea"]
  )
}

data "archive_file" "database_migrations_runner_package" {
  type        = "zip"
  output_path = "${path.module}/build/database-migrations-runner-package.zip"
  source_dir  = "../../../../src"
  excludes    = local.files_to_exclude_from_package
}

resource "google_storage_bucket_object" "database_migrations_runner_package" {
  name                = "database-migrations-runner/${data.archive_file.database_migrations_runner_package.output_md5}-${basename(data.archive_file.database_migrations_runner_package.output_path)}"
  bucket              = data.terraform_remote_state.backends.outputs.cloud_function_packages_bucket_name
  source              = data.archive_file.database_migrations_runner_package.output_path
  content_disposition = "attachment"
  content_type        = "application/zip"
}

resource "google_cloudfunctions2_function" "database_migrations_runner" {
  name        = "${local.name_prefix}-database-migrations-runner"
  location    = var.region
  description = "Postgres Database Migrations Runner"

  build_config {
    runtime     = "dotnet6"
    entry_point = "DfT.ZEV.Services.Migrator.Handler.Function"

    environment_variables = {
      GOOGLE_BUILDABLE = "./services/functions/migrator/DfT.ZEV.Services.Migrator.Handler"
      BUILDID          = var.source_commit_hash
    }

    source {
      storage_source {
        bucket = data.terraform_remote_state.backends.outputs.cloud_function_packages_bucket_name
        object = google_storage_bucket_object.database_migrations_runner_package.name
      }
    }
  }

  service_config {
    min_instance_count               = 0
    max_instance_count               = 1
    max_instance_request_concurrency = 1
    timeout_seconds                  = 5 * 60
    available_memory                 = "2Gi"
    available_cpu                    = "1"

    vpc_connector_egress_settings = "PRIVATE_RANGES_ONLY"
    vpc_connector                 = data.terraform_remote_state.network.outputs.vpc_serverless_connector_id

    all_traffic_on_latest_revision = true
    service_account_email          = google_service_account.database_migrations_runner.email

    environment_variables = {
      Postgres__Host        = data.terraform_remote_state.backends.outputs.postgres_config.host
      Postgres__Port        = "5432",
      Postgres__DbName      = data.terraform_remote_state.backends.outputs.postgres_config.db_name
      Postgres__User        = data.terraform_remote_state.backends.outputs.postgres_config.username
      Postgres__UseSsl      = true
      Postgres__MaxPoolSize = 2
      PGSSLCERT             = "/etc/secrets/postgres-cert/${data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id}"
      PGSSLKEY              = "/etc/secrets/postgres-key/${data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id}"
    }

    secret_environment_variables {
      project_id = var.project
      key        = "Postgres__Password"
      secret     = data.terraform_remote_state.backends.outputs.postgres_config.password_secret_id
      version    = "latest"
    }

    secret_volumes {
      project_id = var.project
      mount_path = "/etc/secrets/postgres-cert"
      secret     = data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id
    }

    secret_volumes {
      project_id = var.project
      mount_path = "/etc/secrets/postgres-key"
      secret     = data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id
    }
  }

  depends_on = [
    # Access to secrets is required to create the function
    google_secret_manager_secret_iam_member.database_migrations_runner_secrets,
  ]
}
