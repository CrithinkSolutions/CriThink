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

# Language
variable "cognitive_account_name" {
  type        = string
  description = "Cognitive account name"
}

variable "keyvault_id" {
  type        = string
  description = "KeyVault resource id"
}
