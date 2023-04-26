resource "azurerm_storage_account" "internal_storage" {
  name                            = var.internal_stg_name
  resource_group_name             = var.rg_name
  location                        = var.rg_location
  account_kind                    = "StorageV2"
  account_tier                    = "Standard"
  access_tier                     = "Hot"
  account_replication_type        = "LRS"
  enable_https_traffic_only       = true
  shared_access_key_enabled       = true
  allow_nested_items_to_be_public = false

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
