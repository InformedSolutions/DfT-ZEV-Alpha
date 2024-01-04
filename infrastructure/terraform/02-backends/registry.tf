resource "google_artifact_registry_repository" "image_repository" {
  location      = var.region
  repository_id = "${local.name_prefix}-repository"
  description   = "Repository for housing application images"
  format        = "DOCKER"
}

locals {
  image_repository_url = "${google_artifact_registry_repository.image_repository.location}-docker.pkg.dev/${google_artifact_registry_repository.image_repository.project}/${google_artifact_registry_repository.image_repository.repository_id}"
}
