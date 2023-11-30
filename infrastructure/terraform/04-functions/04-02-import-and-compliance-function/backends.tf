terraform {
  backend "gcs" {
    prefix = "terraform/functions/import-and-compliance-function"
  }
}

data "terraform_remote_state" "network" {
  backend   = "gcs"
  workspace = terraform.workspace

  config = {
    bucket = var.tf_state_bucket
    prefix = "terraform/network"
  }
}

data "terraform_remote_state" "backends" {
  backend   = "gcs"
  workspace = terraform.workspace

  config = {
    bucket = var.tf_state_bucket
    prefix = "terraform/backends"
  }
}
