resource "google_service_account" "manufacturer_portal" {
  account_id   = "${local.name_prefix}-manufacturer-prtl"
  display_name = "${local.name_prefix}-manufacturer-portal"
  description  = "Service Account for Manufacturer Portal to run in Cloud Run"
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
  location = google_cloud_run_v2_service.manufacturer_portal.location
  project  = google_cloud_run_v2_service.manufacturer_portal.project
  service  = google_cloud_run_v2_service.manufacturer_portal.name

  policy_data = data.google_iam_policy.noauth.policy_data
}
