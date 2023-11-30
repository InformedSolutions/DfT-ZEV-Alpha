output "network" {
  value = module.network
}

output "private_service_access_address_range" {
  description = "Name of the reserved range for private services"
  value = module.cloudsql_private_service_access.google_compute_global_address_name
}
