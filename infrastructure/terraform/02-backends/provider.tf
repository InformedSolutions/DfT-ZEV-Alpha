provider "google" {
  project               = var.project
  region                = var.region
  user_project_override = true
  billing_project       = var.project

  default_labels = {
    project     = "zev"
    environment = var.environment
  }
}
