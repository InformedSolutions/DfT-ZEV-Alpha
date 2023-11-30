terraform {
  backend "gcs" {
    prefix = "terraform/apis/scheme-data-api"
  }
}
