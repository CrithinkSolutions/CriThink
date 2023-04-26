# Generic
variable "rg_name" {
  type        = string
  description = "Resource group name"
}

variable "rg_location" {
  type        = string
  description = "Resource group location"
}

variable "tag_appname" {
  type        = string
  description = "Application name to tag resources with"
  default     = "CriThink"
}

variable "tag_environment" {
  type        = string
  description = "Environment to tag resources with"
}

# KeyVault
variable "keyvault_name" {
  type        = string
  description = "KeyVault name"
}

# AppService
variable "appsrvpln_name" {
  type        = string
  description = "App Service Plan name"
}

variable "appsrv_name" {
  type        = string
  description = "App Service name"
}

variable "acr_name" {
  type        = string
  description = "ACR name"
}

variable "keyvault_ref_acr_user_password" {
  type        = string
  description = "KeyVault reference to the password of the account able to pull images from ACR"
}

variable "aspnet_environment" {
  type        = string
  description = "Asp Net environment value"
}

# KeyVault Policy
variable "objectid_app" {
  type        = string
  description = "App registration object id"
  sensitive   = true
}

# Database
variable "db_server_name" {
  type        = string
  description = "Database server name"
}

variable "db_name" {
  type        = string
  description = "Database name"
}

variable "db_admin_username" {
  type        = string
  description = "Database admin username"
  sensitive   = true
}

variable "db_admin_password" {
  type        = string
  description = "Database admin username"
  sensitive   = true
}

# Redis
variable "redis_name" {
  type        = string
  description = "Redis cache DNS name"
}

# Storage
variable "internal_stg_name" {
  type        = string
  description = "Internal storage account name"
}

# DNS Zone
variable "app_domain" {
  type        = string
  description = "AppService domain"
}

# Roles
variable "acr_id" {
  type        = string
  description = "ACR resource id"
}

# Secrets
variable "acr_user_password" {
  type        = string
  description = "Password of the account able to pull images from ACR"
  sensitive   = true
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
