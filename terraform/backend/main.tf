terraform {

  backend "azurerm" {
    resource_group_name  = "crithink-terraform"
    storage_account_name = "crithinkterraformstg01"
    container_name       = "terraform-states"
    key                  = "core-tfstate.tfstate"
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
  name     = "crithink-terraform"
  location = local.region

  lifecycle {
    prevent_destroy = true
  }

  tags = {
    application_name = local.application_name
    environment      = local.environment
  }
}

resource "azurerm_storage_account" "state" {
  name                      = "crithinkterraformstg01"
  resource_group_name       = azurerm_resource_group.rg.name
  location                  = azurerm_resource_group.rg.location
  account_kind              = "StorageV2"
  account_tier              = "Standard"
  access_tier               = "Hot"
  account_replication_type  = "LRS"
  enable_https_traffic_only = true

  lifecycle {
    prevent_destroy = true
  }

  tags = {
    application_name = local.application_name
    environment      = local.environment
  }
}

resource "azurerm_storage_container" "core-container" {
  name                 = "terraform-states"
  storage_account_name = azurerm_storage_account.state.name

  lifecycle {
    prevent_destroy = true
  }
}

resource "azurerm_management_lock" "resource-group" {
  name       = "crithink-terraform-rg"
  scope      = azurerm_resource_group.rg.id
  lock_level = "CanNotDelete"
  notes      = "Items can't be deleted in this resource group"
}