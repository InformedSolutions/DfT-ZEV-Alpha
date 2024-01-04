# Application data buckets
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

resource "google_storage_bucket" "manufacturer_portal_uploads_unscanned" {
  name     = "${local.name_prefix}-manufacturer-portal-uploads-unscanned"
  location = var.region
  project  = var.project

  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"

  versioning {
    enabled = true
  }
}

resource "google_storage_bucket" "manufacturer_portal_uploads_quarantined" {
  name     = "${local.name_prefix}-manufacturer-portal-uploads-quarantined"
  location = var.region
  project  = var.project

  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"

  versioning {
    enabled = true
  }
}

resource "google_storage_bucket" "manufacturer_portal_uploads_safe" {
  name     = "${local.name_prefix}-manufacturer-portal-uploads-safe"
  location = var.region
  project  = var.project

  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"

  versioning {
    enabled = true
  }
}

# Utility buckets
resource "google_storage_bucket" "cloud_function_packages" {
  name                        = "${var.environment}-cloud-functions-packages"
  location                    = var.region
  storage_class               = "REGIONAL"
  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"
}
