output "function_name" {
  description = "The name of the function"
  value       = google_cloudfunctions2_function.notifications_service.name
}

output "function_url" {
  description = "The URL of the notifications function"
  value       = google_cloudfunctions2_function.notifications_service.url
}
