resource "google_service_account" "compliance_calculation_service" {
  account_id   = "${local.name_prefix}-compl-calc-svc"
  display_name = "${local.name_prefix}-compliance-calculation-service"
  description  = "Service Account for ZEV Compliance Calculation Service to run in Cloud Functions"
}

resource "google_secret_manager_secret_iam_member" "compliance_calculation_service_secrets" {
  for_each = {
    for i, value in flatten([
      google_secret_manager_secret.postgres_password,
      google_secret_manager_secret.postgres_client_certificate,
      google_secret_manager_secret.postgres_client_key,
    ]) : i => value
  }

  project   = var.project
  secret_id = each.value.secret_id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.compliance_calculation_service.email}"
}

resource "google_project_iam_member" "compliance_calculation_service_read_manufacturer_data_objects" {
  project = var.project
  role    = "roles/storage.objectViewer"
  member  = "serviceAccount:${google_service_account.compliance_calculation_service.email}"

  condition {
    title      = "allow-manufacturer-data-bucket"
    expression = "resource.name.startsWith(\"projects/_/buckets/${google_storage_bucket.manufacturer_data.name}\")"
  }
}
