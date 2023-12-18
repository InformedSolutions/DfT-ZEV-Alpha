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

resource "google_project_iam_member" "identity_platform_admin_member" {
  project = var.project
  role    = "roles/firebaseauth.admin"
  member  = "serviceAccount:${google_service_account.manufacturer_portal.email}"
}

resource "google_cloud_tasks_queue_iam_member" "organisation_api_email_notification_queue" {
  name   = data.terraform_remote_state.backends.outputs.email_notifications_queue_name
  role   = "roles/cloudtasks.enqueuer"
  member = "serviceAccount:${google_service_account.manufacturer_portal.email}"
}

resource "google_cloud_run_service_iam_member" "organisation_api_email_notification_invoke" {
  service = data.terraform_remote_state.notifications_function.outputs.function_name
  role    = "roles/run.invoker"
  member  = "serviceAccount:${google_service_account.manufacturer_portal.email}"
}

resource "google_secret_manager_secret_iam_member" "identity_platform_api_key_secret" {
  project   = var.project
  secret_id = data.terraform_remote_state.backends.outputs.identity_platform_config.api_token_secret_id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.manufacturer_portal.email}"
}
