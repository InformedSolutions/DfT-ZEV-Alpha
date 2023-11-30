resource "google_service_account" "scheme_administration_portal" {
  account_id   = "${local.name_prefix}-scheme-admin-prtl"
  display_name = "${local.name_prefix}-scheme-administration-portal"
  description  = "Service Account for Scheme Administration Portal to run in Cloud Run"
}
