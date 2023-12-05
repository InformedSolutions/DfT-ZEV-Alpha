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

variable "compliance_calculation_svc_resource_quotas" {
  type = object({
    max_instance_count               = number,
    max_instance_request_concurrency = number,
    timeout_seconds                  = number,
    available_memory                 = string,
    available_cpu                    = number
  })
  description = "Resource quotas for Cloud Function  Compliance Calculation Service."
  default = {
    max_instance_count               = 1,
    max_instance_request_concurrency = 1
    timeout_seconds                  = 1800,
    available_memory                 = "2Gi"
    available_cpu                    = 1,
  }
}

variable "compliance_calculation_svc_max_db_connections" {
  type        = number
  description = "Maximum size of DB connection pool for each instance of the service"
  default     = 2
}

variable "source_commit_hash" {
  description = "The source commit hash of the function being deployed"
  type        = string
  default     = "latest"
}
