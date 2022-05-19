terraform {

  backend "azurerm" {
    resource_group_name  = "crithink-terraform"
    storage_account_name = "crithinkterraformstg01"
    container_name       = "terraform-states"
    key                  = "stg-tfstate.tfstate"
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "crithink-stg"
  location = local.region

  tags = {
    application_name = local.application_name
    environment      = local.environment
  }
}

module "core_module" {
  source = "../_modules/core-module"

  # Input variables
  # Resources
  rg_name                        = azurerm_resource_group.rg.name
  rg_location                    = azurerm_resource_group.rg.location
  keyvault_name                  = var.keyvault_name
  appsrvpln_name                 = var.appsrvpln_name
  appsrv_name                    = var.appsrv_name
  acr_name                       = var.acr_name
  acr_user_password              = var.acr_user_password
  acr_id                         = var.acr_id
  keyvault_ref_acr_user_password = var.keyvault_ref_acr_user_password
  objectid_app                   = var.objectid_app
  db_server_name                 = var.db_server_name
  db_name                        = var.db_name
  db_admin_username              = var.db_admin_username
  db_admin_password              = var.db_admin_password
  redis_name                     = var.redis_name
  internal_stg_name              = var.internal_stg_name
  app_domain                     = var.app_domain
  aspnet_environment             = var.aspnet_environment

  # Secrets
  jwt_secret_key                        = var.jwt_secret_key
  cross_service_scraper_value           = var.cross_service_scraper_value
  cross_service_dnews_value             = var.cross_service_dnews_value
  authentication_google_client_secret   = var.authentication_google_client_secret
  authentication_google_client_id       = var.authentication_google_client_id
  authentication_facebook_client_secret = var.authentication_facebook_client_secret
  authentication_facebook_client_id     = var.authentication_facebook_client_id
  sendgrid_api_key                      = var.sendgrid_api_key

  # Misc
  tag_appname     = local.application_name
  tag_environment = local.environment
}

module "ai_module" {
  source = "../_modules/ai-module"

  # Input variables
  # Resources
  rg_name     = azurerm_resource_group.rg.name
  rg_location = azurerm_resource_group.rg.location
  ai_name     = var.ai_name
  keyvault_id = module.core_module.keyvault_id

  # Misc
  tag_appname     = local.application_name
  tag_environment = local.environment
}

resource "azurerm_management_lock" "resource_group" {
  name       = "crithink-stg-rg"
  scope      = azurerm_resource_group.rg.id
  lock_level = "CanNotDelete"
  notes      = "Items can't be deleted in this resource group"
}
