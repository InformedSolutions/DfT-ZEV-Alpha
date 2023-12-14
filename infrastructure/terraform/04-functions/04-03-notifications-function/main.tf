locals {
  name_prefix = "${var.environment}-zev"
  files_to_exclude_from_package = setunion(
    fileset("../../../../src", "**/{bin,obj}/**"),
    [".idea"]
  )
}

data "archive_file" "notifications_service_package" {
  type        = "zip"
  output_path = "${path.module}/build/notifications-service-package.zip"
  source_dir  = "../../../../src"
  excludes    = local.files_to_exclude_from_package
}

resource "google_storage_bucket_object" "notifications_service_package" {
  name                = "notifications-service/${data.archive_file.notifications_service_package.output_md5}-${basename(data.archive_file.notifications_service_package.output_path)}"
  bucket              = data.terraform_remote_state.backends.outputs.cloud_function_packages_bucket_name
  source              = data.archive_file.notifications_service_package.output_path
  content_disposition = "attachment"
  content_type        = "application/zip"
}

resource "google_cloudfunctions2_function" "notifications_service" {
  name        = "${local.name_prefix}-notifications-service"
  location    = var.region
  description = "ZEV Notifications Service"

  build_config {
    runtime     = "dotnet6"
    entry_point = "DfT.ZEV.Services.Notifications.Handler.Function"

    environment_variables = {
      GOOGLE_BUILDABLE = "./services/functions/notifications/DfT.ZEV.Services.Notifications.Handler"
      BUILDID          = var.source_commit_hash
    }

    source {
      storage_source {
        bucket = data.terraform_remote_state.backends.outputs.cloud_function_packages_bucket_name
        object = google_storage_bucket_object.notifications_service_package.name
      }
    }
  }

  service_config {
    min_instance_count               = 0
    max_instance_count               = var.notifications_svc_resource_quotas.max_instance_count
    max_instance_request_concurrency = var.notifications_svc_resource_quotas.max_instance_request_concurrency
    timeout_seconds                  = var.notifications_svc_resource_quotas.timeout_seconds
    available_memory                 = var.notifications_svc_resource_quotas.available_memory
    available_cpu                    = var.notifications_svc_resource_quotas.available_cpu

    # TODO: Enable to limit access after configuring Cloud Tasks
    #            ingress_settings = "ALLOW_INTERNAL_ONLY"
    vpc_connector_egress_settings = "PRIVATE_RANGES_ONLY"
    vpc_connector                 = data.terraform_remote_state.network.outputs.vpc_serverless_connector_id

    all_traffic_on_latest_revision = true
    service_account_email          = google_service_account.notifications_service_runner.email

    environment_variables = {
      GoogleCloud__ProjectId = var.project
    }

    secret_environment_variables {
      project_id = var.project
      key        = "GovUkNotifyApiKey"
      secret     = google_secret_manager_secret.govuk_notify_api_key.secret_id
      version    = "latest"
    }
  }
}
