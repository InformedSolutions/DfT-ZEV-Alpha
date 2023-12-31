terraform {
  backend "gcs" {
    prefix = "terraform/backends"
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
