terraform {
  backend "gcs" {
    prefix = "terraform/websites/manufacturer-data-review-website"
  }
}
