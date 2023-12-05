resource "google_secret_manager_secret" "govuk_notify_api_key" {
  secret_id = "${local.name_prefix}-govuk-notify-api-key"

  replication {
    user_managed {
      # Single region for Alpha configuration
      replicas {
        location = var.region
      }
    }
  }
}