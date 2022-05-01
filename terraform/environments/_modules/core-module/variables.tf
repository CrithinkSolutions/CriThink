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

variable "acr_url" {
  type        = string
  description = "ACR URL to pull images from"
}

variable "acr_user_username" {
  type        = string
  description = "Username of the account able to pull images from ACR"
}

variable "keyvault_ref_acr_user_password" {
  type        = string
  description = "KeyVault reference to the password of the account able to pull images from ACR"
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
  type = string
  description = "Redis cache DNS name"
}

# Secrets
variable "acr_user_password" {
  type        = string
  description = "Password of the account able to pull images from ACR"
  sensitive   = true
}
