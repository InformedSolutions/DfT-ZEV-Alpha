locals {
  name_prefix            = "${var.environment}-zev"
  serverless_subnet_name = "serverless-subnet"
}

module "network" {
  source  = "terraform-google-modules/network/google"
  version = "8.0.0"

  project_id   = var.project
  network_name = "${local.name_prefix}-network"

  subnets = [
    {
      subnet_name   = local.serverless_subnet_name
      subnet_ip     = var.serverless_connector_ip_range
      subnet_region = var.region
    }
  ]
}

module "cloudsql_private_service_access" {
  source  = "GoogleCloudPlatform/sql-db/google//modules/private_service_access"
  version = "17.0.1"

  project_id  = var.project
  vpc_network = module.network.network_name

  depends_on = [module.network]
}

resource "google_vpc_access_connector" "serverless_connector" {
  name    = "default-connector"
  project = var.project
  region  = var.region

  subnet {
    project_id = var.project
    name       = module.network.subnets["${var.region}/${local.serverless_subnet_name}"].name
  }

  machine_type  = var.serverless_connector_config.machine_type
  min_instances = var.serverless_connector_config.min_instances
  max_instances = var.serverless_connector_config.max_instances
}
