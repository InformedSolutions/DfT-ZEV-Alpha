resource "google_service_account" "notifications_service_runner" {
  account_id   = "${local.name_prefix}-notifications-function"
  display_name = "${local.name_prefix}-notifications-function"
  description  = "Service Account for Notifications Service to run in Cloud Functions"
}

resource "google_secret_manager_secret_iam_member" "notifications_service_runner_secrets" {
  for_each = toset([
    google_secret_manager_secret.govuk_notify_api_key.secret_id,
  ])

  project   = var.project
  secret_id = each.value
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.notifications_service_runner.email}"
}
