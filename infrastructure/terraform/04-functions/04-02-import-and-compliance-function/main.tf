locals {
  name_prefix = "${var.environment}-zev"
  files_to_exclude_from_package = setunion(
    fileset("../../../../src", "**/{bin,obj}/**"),
    [".idea"]
  )
}

data "archive_file" "compliance_calculation_service_package" {
  type        = "zip"
  output_path = "${path.module}/build/compliance-calculation-service-package.zip"
  source_dir  = "../../../../src"
  excludes    = local.files_to_exclude_from_package
}

resource "google_storage_bucket_object" "compliance_calculation_service_package" {
  name                = "compliance-calculation-service/${data.archive_file.compliance_calculation_service_package.output_md5}-${basename(data.archive_file.compliance_calculation_service_package.output_path)}"
  bucket              = data.terraform_remote_state.backends.outputs.cloud_function_packages_bucket_name
  source              = data.archive_file.compliance_calculation_service_package.output_path
  content_disposition = "attachment"
  content_type        = "application/zip"
}

resource "google_cloudfunctions2_function" "compliance_calculation_service" {
  name        = "${local.name_prefix}-compliance-calculation-service"
  location    = var.region
  description = "ZEV Compliance Calculation Service"

  build_config {
    runtime     = "dotnet6"
    entry_point = "DfT.ZEV.Services.ComplianceCalculation.Handler.Function"

    environment_variables = {
      GOOGLE_BUILDABLE = "./services/functions/compliance-calculation/DfT.ZEV.Services.ComplianceCalculation.Handler"
    }

    source {
      storage_source {
        bucket = data.terraform_remote_state.backends.outputs.cloud_function_packages_bucket_name
        object = google_storage_bucket_object.compliance_calculation_service_package.name
      }
    }
  }

  service_config {
    min_instance_count               = 0
    max_instance_count               = var.compliance_calculation_svc_resource_quotas.max_instance_count
    max_instance_request_concurrency = var.compliance_calculation_svc_resource_quotas.max_instance_request_concurrency
    timeout_seconds                  = var.compliance_calculation_svc_resource_quotas.timeout_seconds
    available_memory                 = var.compliance_calculation_svc_resource_quotas.available_memory
    available_cpu                    = var.compliance_calculation_svc_resource_quotas.available_cpu

    # TODO: Enable to limit access after configuring Cloud Tasks
    #            ingress_settings = "ALLOW_INTERNAL_ONLY"
    vpc_connector_egress_settings = "PRIVATE_RANGES_ONLY"
    vpc_connector                 = data.terraform_remote_state.network.outputs.vpc_serverless_connector_id

    all_traffic_on_latest_revision = true
    service_account_email          = google_service_account.compliance_calculation_service.email

    environment_variables = {
      # Postgres config
      Postgres__Host        = data.terraform_remote_state.backends.outputs.postgres_config.host
      Postgres__Port        = "5432",
      Postgres__DbName      = data.terraform_remote_state.backends.outputs.postgres_config.db_name
      Postgres__User        = data.terraform_remote_state.backends.outputs.postgres_config.username
      Postgres__UseSsl      = true
      Postgres__MaxPoolSize = 2
      PGSSLCERT             = "/etc/secrets/postgres-cert/${data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id}"
      PGSSLKEY              = "/etc/secrets/postgres-key/${data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id}"

      GoogleCloud__ProjectId      = var.project
      Buckets__ManufacturerImport = data.terraform_remote_state.backends.outputs.app_data_buckets.manufacturer_data_bucket_id
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
    google_secret_manager_secret_iam_member.compliance_calculation_service_secrets,
  ]
}
