output "network" {
  value = module.network
}

output "private_service_access_address_range" {
  description = "Name of the reserved range for private services"
  value       = module.cloudsql_private_service_access.google_compute_global_address_name
}

output "vpc_serverless_connector_id" {
  description = "ID of the VPC Serverless Connector"
  value       = google_vpc_access_connector.serverless_connector.id
}
