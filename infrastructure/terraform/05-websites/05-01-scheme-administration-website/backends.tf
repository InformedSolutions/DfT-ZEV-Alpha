terraform {
  backend "gcs" {
    prefix = "terraform/websites/scheme-administration-website"
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

data "terraform_remote_state" "organisation_api" {
  backend   = "gcs"
  workspace = terraform.workspace

  config = {
    bucket = var.tf_state_bucket
    prefix = "terraform/apis/organisation-api"
  }
}

data "terraform_remote_state" "scheme_data_api" {
  backend   = "gcs"
  workspace = terraform.workspace

  config = {
    bucket = var.tf_state_bucket
    prefix = "terraform/apis/scheme-data-api"
  }
}

data "terraform_remote_state" "notifications_function" {
  backend   = "gcs"
  workspace = terraform.workspace

  config = {
    bucket = var.tf_state_bucket
    prefix = "terraform/functions/notifications"
  }
}
