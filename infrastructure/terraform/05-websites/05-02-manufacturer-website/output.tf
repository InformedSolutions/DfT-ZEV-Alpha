output "service_url" {
  description = "Link to the service"
  value       = "http://${module.loadbalancer.external_ip}"
}
