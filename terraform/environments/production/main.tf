terraform {

  backend "azurerm" {
    resource_group_name  = "crithink-terraform"
    storage_account_name = "crithinkterraformstg01"
    container_name       = "terraform-states"
    key                  = "production-tfstate.tfstate"
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
  }
}

variable "app-objectid" {
  type = string
  description = "App registration object id"
  sensitive = true
}

variable "db_admin_username" {
  type = string
  description = "Database sa login username"
  sensitive = true
}

variable "db_admin_password" {
  type = string
  description = "Database sa login password"
  sensitive = true
}

data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "rg" {
  name     = "crithink-prod"
  location = "West Europe"
}

resource "azurerm_key_vault" "keyvault" {
  name                        = "prodkeyvault01"
  resource_group_name         = azurerm_resource_group.rg.name
  location                    = azurerm_resource_group.rg.location
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  sku_name                    = "standard"
}

resource "azurerm_app_service_plan" "plan" {
  name                = "prodplan01"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "appsrv" {
  name                = "prodappsrv01"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  app_service_plan_id = azurerm_app_service_plan.appservice-plan.id

  app_settings = {
    "DOCKER_REGISTRY_SERVER_URL"          = var.docker_registry_url
    "DOCKER_REGISTRY_SERVER_USERNAME"     = var.docker_registry_username
    "DOCKER_REGISTRY_SERVER_PASSWORD"     = var.docker_registry_password
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_key_vault_access_policy" "keyvault-appsrv-accesspolicy" {
  key_vault_id = azurerm_key_vault.keyvault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id

  object_id = azurerm_app_service.appsrv.identity.0.principal_id

  secret_permissions = [
    "Get",
    "List",
  ]
}

resource "azurerm_key_vault_access_policy" "keyvault-sshsapp-accesspolicy" {
  key_vault_id = azurerm_key_vault.keyvault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id

  object_id = var.app-objectid

  secret_permissions = [
    "Get",
    "List",
    "Set",
    "Delete",
    "Recover",
    "Backup",
    "Restore",
    "Purge"
  ]

  key_permissions = [
    "Get",
    "List",
    "Update",
    "Delete",
  ]

  certificate_permissions = [
    "Create",
    "Delete",
    "Deleteissuers",
    "Get",
    "Getissuers",
    "Import",
    "List",
    "Listissuers",
    "Update",
  ]
}

resource "azurerm_postgresql_server" "pg-server" {
  name                         = "prodsql01"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  administrator_login          = var.db_admin_username
  administrator_login_password = var.db_admin_password
  sku_name                     = "B_Gen5_1"
  version                      = "11"
  ssl_enforcement_enabled      = true
  auto_grow_enabled            = false
  storage_mb                   = 5120
}

resource "azurerm_postgresql_database" "pg-database" {
  name                = "proddb01"
  resource_group_name = azurerm_resource_group.rg.name
  charset             = "UTF8"
  collation           = "English_United States.1252"
  server_name         = azurerm_postgresql_server.pg-server.name
}