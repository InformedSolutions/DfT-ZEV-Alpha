locals {
  name_prefix = "${var.environment}-zev"
  deployed_at = timestamp()
}

resource "google_cloud_run_v2_service" "scheme_administration_portal" {
  name     = "${local.name_prefix}-scheme-administration-portal"
  location = var.region
  ingress  = "INGRESS_TRAFFIC_INTERNAL_ONLY"

  template {
    service_account = google_service_account.scheme_administration_portal.email

    scaling {
      min_instance_count = 0
      max_instance_count = var.max_instance_count
    }

    containers {
      image = "${data.terraform_remote_state.backends.outputs.image_repository_url}/zev-administration-portal:${var.source_commit_hash}"

      env {
        name  = "DEPLOYED_AT"
        value = local.deployed_at
      }

      env {
        name  = "BUILDID"
        value = var.source_commit_hash
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
      REGISTRY_URL = data.terraform_remote_state.backends.outputs.image_repository_url
    }
  }
}
