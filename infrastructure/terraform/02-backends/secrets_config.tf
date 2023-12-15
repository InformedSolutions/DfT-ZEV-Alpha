resource "google_secret_manager_secret" "postgres_password" {
  secret_id = "${local.name_prefix}-postgres-password"

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
  secret_data = module.postgres_db.generated_user_password
}

resource "google_secret_manager_secret" "postgres_client_certificate" {
  secret_id = "${local.name_prefix}-postgres-client-certificate"

  replication {
    user_managed {
      # Single region for Alpha configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "postgres_certificate_value" {
  secret      = google_secret_manager_secret.postgres_client_certificate.id
  secret_data = google_sql_ssl_cert.db_client_cert.cert
}

resource "google_secret_manager_secret" "postgres_client_key" {
  secret_id = "${local.name_prefix}-postgres-client-key"

  replication {
    user_managed {
      # Single region for Alpha configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "postgres_key_value" {
  secret      = google_secret_manager_secret.postgres_client_key.id
  secret_data = google_sql_ssl_cert.db_client_cert.private_key
}

resource "google_secret_manager_secret" "identity_platform_api_key" {
  secret_id = "${local.name_prefix}-identity-platform-api-key"

  replication {
    user_managed {
      # Single region for Alpha configuration
      replicas {
        location = var.region
      }
    }
  }
}

resource "google_secret_manager_secret_version" "identity_platform_api_key_value" {
  secret      = google_secret_manager_secret.identity_platform_api_key.id
  secret_data = google_apikeys_key.identity_platform.key_string
}
