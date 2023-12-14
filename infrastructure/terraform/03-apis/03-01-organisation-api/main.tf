locals {
  name_prefix = "${var.environment}-zev"
  deployed_at = timestamp()
}

resource "google_cloud_run_v2_service" "organisation_api" {
  name     = "${local.name_prefix}-organisation-api"
  location = var.region
  ingress  = "INGRESS_TRAFFIC_INTERNAL_ONLY"

  template {
    service_account = google_service_account.organisation_api.email

    scaling {
      min_instance_count = 0
      max_instance_count = var.max_instance_count
    }

    vpc_access {
      connector = data.terraform_remote_state.network.outputs.vpc_serverless_connector_id
      egress    = "PRIVATE_RANGES_ONLY"
    }

    containers {
      image = "${data.terraform_remote_state.backends.outputs.image_repository_url}/zev-organisation-api:${var.source_commit_hash}"

      ports {
        container_port = 80
      }

      startup_probe {
        period_seconds    = 4
        failure_threshold = 5

        http_get {
          path = "/health"
          port = 80
        }
      }

      liveness_probe {
        http_get {
          path = "/health"
          port = 80
        }
      }

      dynamic "env" {
        for_each = local.service_envs
        content {
          name  = env.key
          value = env.value
        }
      }

      env {
        name  = "Services__ManufacturerPortalBaseUrl"
        value = var.manufacturer_website_address
      }

      env {
        name  = "GoogleCloud__ProjectId"
        value = var.project
      }

      env {
        name  = "GoogleCloud__Location"
        value = var.region
      }

      env {
        name  = "GoogleCloud__ApiKey"
        value = data.terraform_remote_state.backends.outputs.identity_platform_config.api_token
      }

      env {
        name  = "GoogleCloud__Tenancy__Admin"
        value = data.terraform_remote_state.backends.outputs.identity_platform_config.administration_tenant_name
      }

      env {
        name  = "GoogleCloud__Tenancy__Manufacturers"
        value = data.terraform_remote_state.backends.outputs.identity_platform_config.manufacturers_tenant_name
      }

      env {
        name  = "GoogleCloud__Token__Issuer"
        value = data.terraform_remote_state.backends.outputs.identity_platform_config.token_issuer
      }

      env {
        name  = "GoogleCloud__Token__Audience"
        value = var.project
      }

      env {
        name  = "GoogleCloud__Queues__Notification__Name"
        value = data.terraform_remote_state.backends.outputs.email_notifications_queue_name
      }

      env {
        name  = "GoogleCloud__Queues__Notification__HandlerUrl"
        value = data.terraform_remote_state.notifications_function.outputs.function_url
      }

      env {
        name = local.db_password_env_name
        value_source {
          secret_key_ref {
            secret  = data.terraform_remote_state.backends.outputs.postgres_config.password_secret_id
            version = "latest"
          }
        }
      }

      dynamic "volume_mounts" {
        for_each = local.db_connection_secret_files
        content {
          name       = volume_mounts.key
          mount_path = volume_mounts.value.mount_point
        }
      }
    }

    dynamic "volumes" {
      for_each = local.db_connection_secret_files
      content {
        name = volumes.key
        secret {
          secret       = volumes.value.secret
          default_mode = 0444
          items {
            version = "latest"
            path    = "value"
            mode    = 0400
          }
        }
      }
    }
  }

  depends_on = [
    null_resource.docker_build,
    # Access to secrets is required to start the container
    google_secret_manager_secret_iam_member.organisation_api_secrets,
  ]
}

resource "null_resource" "docker_build" {
  triggers = {
    always_run = local.deployed_at
  }

  provisioner "local-exec" {
    working_dir = "../../../../src/services/apis/organisations/DfT.ZEV.Services.Organisations.Api"
    command     = "make docker-build && make docker-push"

    environment = {
      REGISTRY_URL       = data.terraform_remote_state.backends.outputs.image_repository_url
      SOURCE_COMMIT_HASH = var.source_commit_hash
    }
  }
}
