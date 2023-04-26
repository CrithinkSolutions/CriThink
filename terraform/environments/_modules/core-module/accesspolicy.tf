resource "azurerm_key_vault_access_policy" "keyvault-app-accesspolicy" {
  key_vault_id = azurerm_key_vault.keyvault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id

  object_id = var.objectid_app

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
}

resource "azurerm_key_vault_access_policy" "keyvault-appsrv-accesspolicy" {
  key_vault_id = azurerm_key_vault.keyvault.id
  tenant_id    = data.azurerm_client_config.current.tenant_id

  object_id = azurerm_linux_web_app.appsrv.identity.0.principal_id

  secret_permissions = [
    "Get",
    "List",
  ]
}
