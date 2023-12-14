resource "google_service_account" "scheme_administration_portal" {
  account_id   = "${local.name_prefix}-scheme-admin-prtl"
  display_name = "${local.name_prefix}-scheme-administration-portal"
  description  = "Service Account for Scheme Administration Portal to run in Cloud Run"
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
  location = google_cloud_run_v2_service.scheme_administration_portal.location
  project  = google_cloud_run_v2_service.scheme_administration_portal.project
  service  = google_cloud_run_v2_service.scheme_administration_portal.name

  policy_data = data.google_iam_policy.noauth.policy_data
}

resource "google_project_iam_member" "identity_platform_admin_member" {
  project = var.project
  role    = "roles/firebaseauth.admin"
  member  = "serviceAccount:${google_service_account.scheme_administration_portal.email}"
}

resource "google_cloud_tasks_queue_iam_member" "organisation_api_email_notification_queue" {
  name   = data.terraform_remote_state.backends.outputs.email_notifications_queue_name
  role   = "roles/cloudtasks.enqueuer"
  member = "serviceAccount:${google_service_account.scheme_administration_portal.email}"
}

resource "google_cloud_run_service_iam_member" "organisation_api_email_notification_invoke" {
  service = data.terraform_remote_state.notifications_function.outputs.function_name
  role    = "roles/run.invoker"
  member  = "serviceAccount:${google_service_account.scheme_administration_portal.email}"
}
