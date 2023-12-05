output "service_name" {
  description = "The name of the service"
  value       = google_cloud_run_v2_service.organisation_api.name
}

output "service_url" {
  description = "The URL of the service"
  value       = google_cloud_run_v2_service.organisation_api.uri
}
