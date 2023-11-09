resource "google_secret_manager_secret" "postgres_password" {
  secret_id = "${var.environment}-zev-postgres-password"

  replication {
    user_managed {
      # Single region for Alpha configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "postgres_password_value" {
  secret      = google_secret_manager_secret.postgres_password.id
  secret_data = "placeholder-value" # TODO: Use in ZEVMITSD-67
}
