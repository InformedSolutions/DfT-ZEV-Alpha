resource "google_service_account" "database_migrations_runner" {
  account_id   = "${local.name_prefix}-db-migr-runner"
  display_name = "${local.name_prefix}-database-migrations-runner"
  description  = "Service Account for Database Migrations Runner to run in Cloud Functions"
}

resource "google_secret_manager_secret_iam_member" "database_migrations_runner_secrets" {
  for_each = toset([
    data.terraform_remote_state.backends.outputs.postgres_config.password_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id,
  ])

  project   = var.project
  secret_id = each.value
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.database_migrations_runner.email}"
}
