output "database_migrations_runner_url" {
  description = "The URL of the database migrations runner function"
  value       = google_cloudfunctions2_function.database_migrations_runner.url
}
