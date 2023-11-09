resource "google_service_account" "compliance_calculation_service" {
  account_id   = "${var.environment}-zev-compl-calc-svc"
  display_name = "${var.environment}-zev-compliance-calculation-service"
  description  = "Service Account for ZEV Compliance Calculation Service to run in Cloud Functions"
}

resource "google_secret_manager_secret_iam_member" "compliance_calculation_service_secrets" {
  for_each = {
    for i, value in flatten([
      google_secret_manager_secret.postgres_password,
    ]) : i => value
  }

  project   = var.project
  secret_id = each.value.secret_id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.compliance_calculation_service.email}"
}
