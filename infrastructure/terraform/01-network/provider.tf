provider "google" {
  project = var.project
  region  = var.region

  default_labels = {
    project     = "zev"
    environment = var.environment
  }
}
