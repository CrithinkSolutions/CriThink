locals {
  region           = "Germany West Central"
  environment      = "Staging"
  application_name = "CriThink"
}

variable "acr_name" {
  type        = string
  description = "ACR URL to pull images from"
}

variable "acr_user_password" {
  type        = string
  description = "Password of the account able to pull images from ACR"
  sensitive   = true
}

variable "acr_id" {
  type        = string
  description = "ACR resource id"
}

variable "keyvault_ref_acr_user_password" {
  type        = string
  description = "KeyVault secret name containing the password of the account able to pull images from ACR"
}

variable "objectid_app" {
  type        = string
  description = "App registration object id"
  sensitive   = true
}

variable "db_admin_username" {
  type        = string
  description = "Database sa login username"
  sensitive   = true
}

variable "db_admin_password" {
  type        = string
  description = "Database sa login password"
  sensitive   = true
}

variable "db_name" {
  type        = string
  description = "Database name"
}

variable "db_server_name" {
  type        = string
  description = "Database server name"
}

variable "cognitive_account_name" {
  type        = string
  description = "Cognitive account name"
}

variable "redis_name" {
  type        = string
  description = "Redis Cache name"
}

variable "ai_name" {
  type        = string
  description = "AI resource name"
}

variable "internal_stg_name" {
  type        = string
  description = "Internal storage account name"
}

variable "app_domain" {
  type        = string
  description = "AppService domain"
}

variable "keyvault_name" {
  type        = string
  description = "KeyVault name"
}

variable "appsrvpln_name" {
  type        = string
  description = "App Service Plan name"
}

variable "appsrv_name" {
  type        = string
  description = "App Service name"
}

variable "jwt_secret_key" {
  type        = string
  description = "Password of the account able to pull images from ACR"
  sensitive   = true
}

variable "cross_service_scraper_value" {
  type        = string
  description = "CrossService Scraper value"
  sensitive   = true
}

variable "cross_service_dnews_value" {
  type        = string
  description = "CrossService Debunking News value"
  sensitive   = true
}

variable "authentication_google_client_secret" {
  type        = string
  description = "Google authentication client secret"
  sensitive   = true
}

variable "authentication_google_client_id" {
  type        = string
  description = "Google authentication client id"
  sensitive   = true
}

variable "authentication_facebook_client_secret" {
  type        = string
  description = "Facebook authentication client secret"
  sensitive   = true
}

variable "authentication_facebook_client_id" {
  type        = string
  description = "Facebook authentication client id"
  sensitive   = true
}

variable "sendgrid_api_key" {
  type        = string
  description = "SendGrid API key"
  sensitive   = true
}
