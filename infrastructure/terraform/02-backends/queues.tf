resource "google_cloud_tasks_queue" "email_notifications" {
  name     = "${local.name_prefix}-email-notifications"
  location = var.region

  rate_limits {
    max_dispatches_per_second = 50
  }

  retry_config {
    max_attempts       = 20
    max_retry_duration = "0s"
    min_backoff        = "60s"
    max_backoff        = "3600s"
    max_doublings      = 16
  }

  stackdriver_logging_config {
    sampling_ratio = 1.0
  }
}
