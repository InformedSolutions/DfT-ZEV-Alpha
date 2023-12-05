resource "google_identity_platform_config" "default" {
  project = var.project

  authorized_domains         = var.identity_platform_authorized_redirect_domains
  autodelete_anonymous_users = false

  sign_in {
    allow_duplicate_emails = false

    email {
      enabled           = true
      password_required = true
    }
  }
}

resource "google_identity_platform_tenant" "administration" {
  display_name          = "administration"
  allow_password_signup = true
}

resource "google_identity_platform_tenant" "manufacturers" {
  display_name          = "manufacturers"
  allow_password_signup = true
}

resource "google_apikeys_key" "identity_platform" {
  name         = "${local.name_prefix}-identity-platform-access"
  display_name = "${local.name_prefix}-identity-platform-access"
  project      = var.project

  restrictions {
    api_targets {
      service = ""
      methods = ["*"]
    }
  }
}

import {
  id = "projects/${var.project}/config"
  to = google_identity_platform_config.default
}

import {
  id = "projects/${var.project}/tenants/administration-orudw"
  to = google_identity_platform_tenant.administration
}

import {
  id = "projects/${var.project}/tenants/manufacturers-k624k"
  to = google_identity_platform_tenant.manufacturers
}
