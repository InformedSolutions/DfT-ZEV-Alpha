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
