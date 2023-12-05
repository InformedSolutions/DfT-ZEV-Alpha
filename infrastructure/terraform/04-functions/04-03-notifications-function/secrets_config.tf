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

resource "google_secret_manager_secret_version" "govuk_notify_api_key_value" {
  secret      = google_secret_manager_secret.govuk_notify_api_key.id
  secret_data = var.govuk_notify_api_key
}