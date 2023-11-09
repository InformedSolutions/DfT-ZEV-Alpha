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

variable "resource_quotas_compliance_calculation_svc" {
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
    timeout_seconds                  = 60,
    available_memory                 = "128Mi"
    available_cpu                    = 0.083
  }
}
