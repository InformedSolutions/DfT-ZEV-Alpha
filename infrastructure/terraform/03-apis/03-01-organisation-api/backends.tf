terraform {
  backend "gcs" {
    prefix = "terraform/apis/organisation-api"
  }
}
