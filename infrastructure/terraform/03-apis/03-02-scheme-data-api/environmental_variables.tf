locals {
  db_connection_envs = {
    Postgres__Host        = data.terraform_remote_state.backends.outputs.postgres_config.host
    Postgres__Port        = "5432",
    Postgres__DbName      = data.terraform_remote_state.backends.outputs.postgres_config.db_name
    Postgres__User        = data.terraform_remote_state.backends.outputs.postgres_config.username
    Postgres__UseSsl      = true
    Postgres__MaxPoolSize = var.postgres_connection_pool_size_per_instance
    PGSSLCERT             = "/secrets/postgres-cert/value"
    PGSSLKEY              = "/secrets/postgres-key/value"
  }
  db_password_env_name = "Postgres__Password"

  service_envs = merge(
    local.db_connection_envs,
    {
      DEPLOYED_AT = local.deployed_at
      BUILDID     = var.source_commit_hash

      GoogleCloud__ProjectId = var.project
  })

  db_connection_secret_files = {
    secret_postgres_client_certificate = {
      secret      = data.terraform_remote_state.backends.outputs.postgres_config.client_certificate_secret_id,
      mount_point = "/secrets/postgres-cert"
    },
    secret_postgres_client_key = {
      secret      = data.terraform_remote_state.backends.outputs.postgres_config.client_key_secret_id,
      mount_point = "/secrets/postgres-key"
    }
  }
}
