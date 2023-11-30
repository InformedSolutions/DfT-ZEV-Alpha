resource "google_artifact_registry_repository" "image_repository" {
  location      = var.region
  repository_id = "${local.name_prefix}-repository"
  description   = "Repository for housing application images"
  format        = "DOCKER"
}
