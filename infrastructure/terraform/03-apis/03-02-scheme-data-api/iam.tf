resource "google_service_account" "scheme_data_api" {
  account_id   = "${local.name_prefix}-scheme-data-api"
  display_name = "${local.name_prefix}-scheme-data-api"
  description  = "Service Account for Scheme Data API to run in Cloud Run"
}

resource "google_secret_manager_secret_iam_member" "scheme_data_api_secrets" {
  for_each = toset([
    data.terraform_remote_state.backends.outputs.postgres_config.password_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id,
  ])

  project   = var.project
  secret_id = each.value
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.scheme_data_api.email}"
}

data "google_iam_policy" "noauth" {
  binding {
    role = "roles/run.invoker"
    members = [
      "allUsers",
    ]
  }
}

resource "google_cloud_run_service_iam_policy" "service_noauth" {
  location = google_cloud_run_v2_service.scheme_data_api.location
  project  = google_cloud_run_v2_service.scheme_data_api.project
  service  = google_cloud_run_v2_service.scheme_data_api.name

  policy_data = data.google_iam_policy.noauth.policy_data
}
