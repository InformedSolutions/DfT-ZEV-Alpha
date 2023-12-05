resource "google_service_account" "organisation_api" {
  account_id   = "${local.name_prefix}-organisation-api"
  display_name = "${local.name_prefix}-organisation-api"
  description  = "Service Account for Organisation API to run in Cloud Run"
}

resource "google_secret_manager_secret_iam_member" "organisation_api_secrets" {
  for_each = toset([
    data.terraform_remote_state.backends.outputs.postgres_config.password_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id,
  ])

  project   = var.project
  secret_id = each.value
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.organisation_api.email}"
}
