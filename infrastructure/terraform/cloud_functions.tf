resource "google_storage_bucket" "cloud_function_packages" {
  name                        = "${var.environment}-cloud-functions-packages"
  location                    = var.region
  storage_class               = "REGIONAL"
  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"
}

data "archive_file" "compliance_calculation_service_package" {
  type        = "zip"
  output_path = "${path.module}/build/compliance-calculation-service-package.zip"
  source_dir  = "../../src"
  excludes    = []
}

resource "google_storage_bucket_object" "compliance_calculation_service_package" {
  name                = "compliance-calculation-service/${data.archive_file.compliance_calculation_service_package.output_md5}-${basename(data.archive_file.compliance_calculation_service_package.output_path)}"
  bucket              = google_storage_bucket.cloud_function_packages.name
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
    entry_point = "Zev.Services.ComplianceCalculationService.Handler.Function"

    environment_variables = {
      GOOGLE_BUILDABLE = "./Zev/Zev.Services.ComplianceCalculationService.Handler"
    }

    source {
      storage_source {
        bucket = google_storage_bucket.cloud_function_packages.name
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
    vpc_connector                 = google_vpc_access_connector.serverless_connector.id

    all_traffic_on_latest_revision = true
    service_account_email          = google_service_account.compliance_calculation_service.email

    environment_variables = {
      # Postgres config
      Postgres__Host        = module.postgres_db.private_ip_address
      Postgres__Port        = "5432",
      Postgres__User        = var.database_username
      Postgres__DbName      = local.database_name
      Postgres__UseSsl      = true
      Postgres__MaxPoolSize = var.compliance_calculation_svc_max_db_connections
      PGSSLCERT             = "/etc/secrets/postgres-cert/${google_secret_manager_secret.postgres_client_certificate.secret_id}"
      PGSSLKEY              = "/etc/secrets/postgres-key/${google_secret_manager_secret.postgres_client_key.secret_id}"

      Manufacturer_Data_Bucket_Name = google_storage_bucket.manufacturer_data.id
    }

    secret_environment_variables {
      project_id = var.project
      key        = "Postgres__Password"
      secret     = google_secret_manager_secret.postgres_password.secret_id
      version    = "latest"
    }

    secret_volumes {
      project_id = var.project
      mount_path = "/etc/secrets/postgres-cert"
      secret     = google_secret_manager_secret.postgres_client_certificate.secret_id
    }

    secret_volumes {
      project_id = var.project
      mount_path = "/etc/secrets/postgres-key"
      secret     = google_secret_manager_secret.postgres_client_key.secret_id
    }
  }

  depends_on = [
    # Access to secrets is required to create the function
    google_secret_manager_secret_iam_member.compliance_calculation_service_secrets,
  ]
}
