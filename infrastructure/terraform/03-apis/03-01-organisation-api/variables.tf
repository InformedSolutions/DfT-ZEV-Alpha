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

variable "postgres_connection_pool_size_per_instance" {
  type        = number
  description = "Maximum number of connections to the database per instance."
  default     = 2
}

variable "source_commit_hash" {
  description = "The docker image to deploy"
  type        = string
  default     = "latest"
}

variable "manufacturer_website_address" {
  description = "The base URL for the manufacturer website component."
  type        = string
  default     = "http://34.144.196.181"
}
