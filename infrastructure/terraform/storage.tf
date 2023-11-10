resource "google_storage_bucket" "manufacturer_data" {
  name     = "${local.name_prefix}-manufacturer-data"
  location = var.region
  project  = var.project

  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"

  versioning {
    enabled = true
  }
}
