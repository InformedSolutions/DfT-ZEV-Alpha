terraform {
  backend "gcs" {
    prefix = "terraform/network"
  }
}
