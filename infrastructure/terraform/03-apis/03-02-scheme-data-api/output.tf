output "service_name" {
  description = "The name of the service"
  value       = google_cloud_run_v2_service.scheme_data_api.name
}

output "service_url" {
  description = "The URL of the service"
  value       = google_cloud_run_v2_service.scheme_data_api.uri
}
