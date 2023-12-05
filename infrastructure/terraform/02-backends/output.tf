output "postgres_config" {
  description = "The configuration of Postgres database to be used by the applications"
  value = {
    host     = module.postgres_db.private_ip_address
    db_name  = local.database_name
    username = var.database_username

    password_secret_id           = google_secret_manager_secret.postgres_password.secret_id
    client_certificate_secret_id = google_secret_manager_secret.postgres_client_certificate.secret_id
    client_key_secret_id         = google_secret_manager_secret.postgres_client_key.secret_id
  }
}

output "cloud_function_packages_bucket_name" {
  description = "The name of the bucket used to store Cloud Function packages"
  value       = google_storage_bucket.cloud_function_packages.name
}

output "app_data_buckets" {
  description = "The names of the buckets used to store application data"
  value = {
    manufacturer_data_bucket_id = google_storage_bucket.manufacturer_data.id
  }
}

output "image_repository_url" {
  description = "The URL of the Docker image repository"
  value       = "${google_artifact_registry_repository.image_repository.location}-docker.pkg.dev/${google_artifact_registry_repository.image_repository.project}/${google_artifact_registry_repository.image_repository.repository_id}"
}
