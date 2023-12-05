resource "google_compute_ssl_policy" "default" {
  name            = "${local.name_prefix}-ssl-policy"
  profile         = "MODERN"
  min_tls_version = "TLS_1_2"
}
