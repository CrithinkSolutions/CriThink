terraform {

  backend "azurerm" {
    resource_group_name  = "crithink-terraform"
    storage_account_name = "crithinkterraformstg01"
    container_name       = "terraform-states"
    key                  = "global-tfstate.tfstate"
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
  name     = "crithink-global"
  location = local.region

  lifecycle {
    prevent_destroy = true
  }

  tags = {
    application_name = local.application_name
    environment      = local.environment
  }
}

resource "azurerm_container_registry" "acr" {
  name                = "crithinkacr01"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Basic"
  admin_enabled       = true

  lifecycle {
    prevent_destroy = true
  }

  tags = {
    application_name = local.application_name
    environment      = local.environment
  }
}

resource "azurerm_management_lock" "resource-group" {
  name       = "crithink-global-rg"
  scope      = azurerm_resource_group.rg.id
  lock_level = "CanNotDelete"
  notes      = "Items can't be deleted in this resource group"
}
