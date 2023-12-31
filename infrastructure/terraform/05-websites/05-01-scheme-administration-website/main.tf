locals {
  name_prefix = "${var.environment}-zev"
  deployed_at = timestamp()
}

resource "google_cloud_run_v2_service" "scheme_administration_portal" {
  name     = "${local.name_prefix}-scheme-administration-portal"
  location = var.region
  ingress  = "INGRESS_TRAFFIC_INTERNAL_LOAD_BALANCER"

  template {
    service_account = google_service_account.scheme_administration_portal.email

    scaling {
      min_instance_count = 0
      max_instance_count = var.max_instance_count
    }

    vpc_access {
      connector = data.terraform_remote_state.network.outputs.vpc_serverless_connector_id
      egress    = "ALL_TRAFFIC"
    }

    containers {
      image = "${data.terraform_remote_state.backends.outputs.image_repository_url}/zev-administration-portal:${var.source_commit_hash}"

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

      env {
        name  = "DEPLOYED_AT"
        value = local.deployed_at
      }

      env {
        name  = "BUILDID"
        value = var.source_commit_hash
      }

      # TODO: setup users
      env {
        name  = "BasicAuth__IsEnabled"
        value = var.enable_basic_auth
      }

      env {
        name  = "Buckets__PortalUploadsSafe"
        value = data.terraform_remote_state.backends.outputs.app_data_buckets.manufacturer_portal_uploads_safe_id
      }

      env {
        name  = "Services__OrganisationApiBaseUrl"
        value = data.terraform_remote_state.organisation_api.outputs.service_url
      }

      env {
        name  = "Services__SchemeDataApiBaseUrl"
        value = data.terraform_remote_state.scheme_data_api.outputs.service_url
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
        name  = "GoogleCloud__Tenancy__Admin"
        value = data.terraform_remote_state.backends.outputs.identity_platform_config.administration_tenant_name
      }

      env {
        name  = "GoogleCloud__Tenancy__AppTenant"
        value = data.terraform_remote_state.backends.outputs.identity_platform_config.administration_tenant_name
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
        name = "GoogleCloud__ApiKey"
        value_source {
          secret_key_ref {
            secret  = data.terraform_remote_state.backends.outputs.identity_platform_config.api_token_secret_id
            version = "latest"
          }
        }
      }
    }
  }

  depends_on = [
    null_resource.docker_build,
    # Access to secrets is required to start the container
    google_secret_manager_secret_iam_member.identity_platform_api_key_secret,
  ]
}

resource "null_resource" "docker_build" {
  triggers = {
    always_run = local.deployed_at
  }

  provisioner "local-exec" {
    working_dir = "../../../../src/services/websites/zev-administration-portal"
    command     = "make docker-build && make docker-push"

    environment = {
      REGISTRY_URL       = data.terraform_remote_state.backends.outputs.image_repository_url
      SOURCE_COMMIT_HASH = var.source_commit_hash
    }
  }
}
