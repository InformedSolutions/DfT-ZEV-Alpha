output "postgres_config" {
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
  value = google_storage_bucket.cloud_function_packages.name
}
