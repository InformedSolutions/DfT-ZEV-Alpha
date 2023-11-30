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

variable "serverless_connector_config" {
  type = object({
    machine_type  = string
    min_instances = number
    max_instances = number
  })
  description = "Configuration of Serverless VPC Access connector"
  default = {
    machine_type  = "e2-micro"
    min_instances = 2
    max_instances = 3
  }

  validation {
    condition     = var.serverless_connector_config.min_instances > 1 && var.serverless_connector_config.max_instances > var.serverless_connector_config.min_instances
    error_message = "At least 2 instances must be configured and max instances count must be greater than min instances count."
  }
}

variable "serverless_connector_ip_range" {
  type        = string
  description = "IP range for Serverless VPC Access Connector"
  default     = "10.64.0.0/28" # CIDR block with "/28" netmask is required
}
