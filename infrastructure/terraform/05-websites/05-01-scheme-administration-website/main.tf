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

      # TODO: Add startup and liveness probe

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
        value = true
      }

      env {
        name  = "Services__OrganisationApiBaseUrl"
        value = data.terraform_remote_state.organisation_api.outputs.service_url
      }

      env {
        name  = "GoogleCloud__ProjectId"
        value = var.project
      }

      env {
        name  = "GoogleCloud__ApiKey"
        value = "TODO" # TODO: fill or switch to IAM
      }

      env {
        name  = "GoogleCloud__Tenancy__Admin"
        value = "TODO" # TODO: use valid value
      }

      env {
        name  = "GoogleCloud__Tenancy__Manufacturers"
        value = "TODO" # TODO: use valid value
      }

      env {
        name  = "GoogleCloud__Token__Issuer"
        value = "TODO" # TODO: use valid value
      }

      env {
        name  = "GoogleCloud__Token__Audience"
        value = "TODO" # TODO: use valid value
      }
    }
  }

  depends_on = [
    null_resource.docker_build,
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
