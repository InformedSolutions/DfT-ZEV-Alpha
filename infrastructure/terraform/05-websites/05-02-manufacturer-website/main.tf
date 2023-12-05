locals {
  name_prefix = "${var.environment}-zev"
  deployed_at = timestamp()
}

resource "google_cloud_run_v2_service" "manufacturer_portal" {
  name     = "${local.name_prefix}-manufacturer-portal"
  location = var.region
  ingress  = "INGRESS_TRAFFIC_INTERNAL_LOAD_BALANCER"

  template {
    service_account = google_service_account.manufacturer_portal.email

    scaling {
      min_instance_count = 0
      max_instance_count = var.max_instance_count
    }

    containers {
      image = "${data.terraform_remote_state.backends.outputs.image_repository_url}/zev-manufacturer-portal:${var.source_commit_hash}"

      ports {
        container_port = 80
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
        value = true
      }

      # TODO: Add startup and liveness probe
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
    working_dir = "../../../../src/services/websites/manufacturer-data-review-portal"
    command     = "make docker-build && make docker-push"

    environment = {
      REGISTRY_URL = data.terraform_remote_state.backends.outputs.image_repository_url
      COMMIT_HASH  = var.source_commit_hash
    }
  }
}
