output "image_repository_url" {
  description = "The URL of the Docker image repository"
  value       = "${google_artifact_registry_repository.image_repository.location}-docker.pkg.dev/${google_artifact_registry_repository.image_repository.project}/${google_artifact_registry_repository.image_repository.repository_id}"
}

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

output "identity_platform_config" {
  description = "The configuration of Identity Platform to be used by the applications"
  sensitive   = true
  value = {
    api_token                  = google_apikeys_key.identity_platform.key_string
    administration_tenant_name = google_identity_platform_tenant.administration.name
    manufacturers_tenant_name  = google_identity_platform_tenant.manufacturers.name
    token_issuer               = "https://securetoken.google.com/${var.project}"
  }
}

output "email_notifications_queue_name" {
  description = "The ID of the Cloud Tasks queue used to send email notifications"
  value       = google_cloud_tasks_queue.email_notifications.name
}
