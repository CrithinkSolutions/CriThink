locals {
  region           = "Germany West Central"
  environment      = "Staging"
  application_name = "CriThink"
}

variable "acr_url" {
  type        = string
  description = "ACR URL to pull images from"
}

variable "acr_user_username" {
  type        = string
  description = "Username of the account able to pull images from ACR"
}

variable "acr_user_password" {
  type        = string
  description = "Password of the account able to pull images from ACR"
  sensitive   = true
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
