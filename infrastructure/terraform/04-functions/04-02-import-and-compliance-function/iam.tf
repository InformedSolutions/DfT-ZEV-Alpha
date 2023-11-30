resource "google_service_account" "compliance_calculation_service" {
  account_id   = "${local.name_prefix}-compl-calc-svc"
  display_name = "${local.name_prefix}-compliance-calculation-service"
  description  = "Service Account for ZEV Compliance Calculation Service to run in Cloud Functions"
}

resource "google_secret_manager_secret_iam_member" "compliance_calculation_service_secrets" {
  for_each = toset([
    data.terraform_remote_state.backends.outputs.postgres_config.password_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id,
    data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id,
  ])

  project   = var.project
  secret_id = each.value
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.compliance_calculation_service.email}"
}

resource "google_project_iam_member" "compliance_calculation_service_read_manufacturer_data_objects" {
  project = var.project
  role    = "roles/storage.objectViewer"
  member  = "serviceAccount:${google_service_account.compliance_calculation_service.email}"

  condition {
    title      = "allow-manufacturer-data-bucket"
    expression = "resource.name.startsWith(\"projects/_/buckets/${data.terraform_remote_state.backends.outputs.app_data_buckets.manufacturer_data_bucket_id}\")"
  }
}
