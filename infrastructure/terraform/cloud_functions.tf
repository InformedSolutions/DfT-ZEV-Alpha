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
  name        = "${var.environment}-zev-compliance-calculation-service"
  location    = var.region
  description = "ZEV Compliance Calculation Service"

  build_config {
    runtime     = "dotnet6"
    entry_point = "Zev.Services.Importer.Function.Function"

    environment_variables = {
      GOOGLE_BUILDABLE = "./Zev/Zev.Services.Importer.Function"
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
    max_instance_count               = var.resource_quotas_compliance_calculation_svc.max_instance_count
    max_instance_request_concurrency = var.resource_quotas_compliance_calculation_svc.max_instance_request_concurrency
    timeout_seconds                  = var.resource_quotas_compliance_calculation_svc.timeout_seconds
    available_memory                 = var.resource_quotas_compliance_calculation_svc.available_memory
    available_cpu                    = var.resource_quotas_compliance_calculation_svc.available_cpu

    # TODO: Enable to limit access after configuring Cloud Tasks
    #            ingress_settings = "ALLOW_INTERNAL_ONLY"
    #        vpc_connector_egress_settings = # TODO: Set in ZEVMITSD-67
    #        vpc_connector                 = # TODO: Set in ZEVMITSD-67

    all_traffic_on_latest_revision = true
    service_account_email          = google_service_account.compliance_calculation_service.email


    environment_variables = {
      Postgres__Host = "placeholder-value" # TODO: Use in ZEVMITSD-67
    }

    secret_environment_variables {
      project_id = var.project
      key        = "Postgres__Password"
      secret     = google_secret_manager_secret.postgres_password.secret_id
      version    = "latest"
    }

    secret_volumes {
      project_id = var.project
      mount_path = "/etc/secrets"                                           # TODO: Adjust in ZEVMITSD-67
      secret     = google_secret_manager_secret.postgres_password.secret_id # TODO: Adjust in ZEVMITSD-67
    }
  }
}
