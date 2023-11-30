resource "google_bigquery_connection" "postgres_database_connection" {
  connection_id = "ZEV-database"
  friendly_name = "ZEV-database"
  description   = "Access to ZEV Postgres database"
  location      = var.region

  cloud_sql {
    instance_id = module.postgres_db.instance_connection_name
    database    = local.database_name
    type        = "POSTGRES"
    credential {
      username = var.database_username
      password = module.postgres_db.generated_user_password
    }
  }
}

resource "google_project_iam_member" "bigquery_allow_cloudsql_connection" {
  project = var.project
  role    = "roles/cloudsql.client"
  member  = "serviceAccount:${google_bigquery_connection.postgres_database_connection.cloud_sql[0].service_account_id}"
}
