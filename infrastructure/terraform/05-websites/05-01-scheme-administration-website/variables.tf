variable "tf_state_bucket" {
  type        = string
  description = "Bucket in which terraform state of other layers is held"
}

variable "region" {
  type        = string
  description = "GCP region to which resources will be deployed."
  default     = "europe-west1"
}

variable "project" {
  type        = string
  description = "GCP project ID to which resources will be deployed."
  nullable    = false
}

variable "environment" {
  type        = string
  description = "Name of the environment."
  nullable    = false
}

variable "max_instance_count" {
  type        = number
  description = "Maximum number of instances to be started."
  default     = 50
}

variable "source_commit_hash" {
  description = "The docker image to deploy"
  type        = string
  default     = "latest"
}

variable "enable_basic_auth" {
  description = "Whether basic authentication is enabled to access the service."
  type        = bool
  default     = false
}