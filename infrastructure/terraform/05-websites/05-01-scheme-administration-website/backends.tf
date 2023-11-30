terraform {
  backend "gcs" {
    prefix = "terraform/websites/scheme-administration-website"
  }
}
