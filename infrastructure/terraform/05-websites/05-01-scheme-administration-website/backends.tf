terraform {
  backend "gcs" {
    prefix = "terraform/websites/scheme-administration-website"
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
