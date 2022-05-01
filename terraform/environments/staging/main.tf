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

  lifecycle {
    prevent_destroy = true
  }

  tags = {
    application_name = local.application_name
    environment      = local.environment
  }
}

module "core-module" {
  source = "../_modules/core-module"

  # Input variables
  rg_name                        = azurerm_resource_group.rg.name
  rg_location                    = azurerm_resource_group.rg.location
  keyvault_name                  = "crithinkstgkeyvault01"
  appsrvpln_name                 = "demoplan01"
  appsrv_name                    = "crithink-demo"
  acr_url                        = var.acr_url
  acr_user_username              = var.acr_user_username
  acr_user_password              = var.acr_user_password
  keyvault_ref_acr_user_password = var.keyvault_ref_acr_user_password
  objectid_app                   = var.objectid_app
  db_server_name                 = var.db_server_name
  db_name                        = var.db_name
  db_admin_username              = var.db_admin_username
  db_admin_password              = var.db_admin_password
  redis_name                     = var.redis_name
  tag_appname                    = local.application_name
  tag_environment                = local.environment
}

module "cognitive-module" {
  source = "../_modules/cognitive-module"

  # Input variables
  rg_name                = azurerm_resource_group.rg.name
  rg_location            = azurerm_resource_group.rg.location
  cognitive_account_name = var.cognitive_account_name
  tag_appname            = local.application_name
  tag_environment        = local.environment
}

module "ai-module" {
  source = "../_modules/ai-module"

  # Input variables
  rg_name         = azurerm_resource_group.rg.name
  rg_location     = azurerm_resource_group.rg.location
  ai_name         = var.ai_name
  tag_appname     = local.application_name
  tag_environment = local.environment
}

resource "azurerm_management_lock" "resource-group" {
  name       = "crithink-stg-rg"
  scope      = azurerm_resource_group.rg.id
  lock_level = "CanNotDelete"
  notes      = "Items can't be deleted in this resource group"
}
