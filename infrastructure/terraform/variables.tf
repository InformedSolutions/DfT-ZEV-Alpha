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

variable "database_zone" {
  type        = string
  description = "Primary zone for the Postgres database"
  default     = "europe-west1-b"
}

variable "database_availability_type" {
  type        = string
  description = "Availability of Postgres database instance"
  default     = "ZONAL"
}

variable "database_instance_type" {
  type        = string
  description = "Type of Postgres database instance"
  default     = "db-f1-micro"
}

variable "database_disk_initial_size" {
  type        = string
  description = "Initial size of the Postgres databases disk"
  default     = 10
}

variable "database_disk_autoresize_limit" {
  type        = string
  description = "Upper limit for Postgres database disk auto resize"
  default     = 30
}

variable "database_max_connections" {
  type        = number
  description = "Maximum number of connections allowed by the Postgres database"
  # 25 is Cloud SQL's default value for tiny instance (https://cloud.google.com/sql/docs/postgres/flags#postgres-m)
  default = 25
}

variable "database_backups_pitr_enabled" {
  type        = bool
  description = "Enables point-in-time recovery for Postgres database"
  default     = true
}

variable "database_backups_pitr_days" {
  type        = string
  description = "Retention policy that determines how many days of transaction logs are stored for point-in-time recovery"
  default     = 2
}

variable "database_backups_number_of_stored_backups" {
  type        = string
  description = "Retention policy that determines how many daily backups of Postgres database are stored"
  default     = 14
}

variable "database_username" {
  type        = string
  description = "Username for Postgres database"
  default     = "zev"
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
    timeout_seconds                  = 60,
    available_memory                 = "128Mi"
    available_cpu                    = 0.083,
  }
}

variable "compliance_calculation_svc_max_db_connections" {
  type        = number
  description = "Maximum size of DB connection pool for each instance of the service"
  default     = 2
}
