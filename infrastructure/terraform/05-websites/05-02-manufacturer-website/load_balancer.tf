resource "google_compute_global_address" "service_load_balancer_ip" {
  name = "${local.name_prefix}-manufacturer-portal-lb-ip"

  lifecycle {
    prevent_destroy = true
  }
}

resource "google_compute_region_network_endpoint_group" "search_service_serverless_neg" {
  name                  = "${local.name_prefix}-manufacturer-portal-serverless-neg"
  network_endpoint_type = "SERVERLESS"
  region                = var.region

  cloud_run {
    service = google_cloud_run_v2_service.manufacturer_portal.name
  }
}

module "loadbalancer" {
  source  = "GoogleCloudPlatform/lb-http/google//modules/serverless_negs"
  version = "10.0.0"
  name    = "${local.name_prefix}-manufacturer-portal-lb"
  project = var.project

  address        = google_compute_global_address.service_load_balancer_ip.address
  create_address = false

  # TODO: setup domain and SSL certificate
  #  ssl        = false
  #  ssl_policy = data.terraform_remote_state.network.outputs.ssl_policy_id
  #  https_redirect                  = true
  #  managed_ssl_certificate_domains = [var.domain_name]

  backends = {
    default = {
      description = "Manufacturer Portal"

      groups = [
        {
          group = google_compute_region_network_endpoint_group.search_service_serverless_neg.id
        }
      ]

      enable_cdn              = false
      protocol                = null
      port_name               = null
      security_policy         = null
      custom_request_headers  = null
      custom_response_headers = null
      compression_mode        = null
      security_policy         = null
      edge_security_policy    = null

      iap_config = {
        enable               = false
        oauth2_client_id     = ""
        oauth2_client_secret = ""
      }

      log_config = {
        enable      = false
        sample_rate = null
      }
    }
  }
}
