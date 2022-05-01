resource "azurerm_key_vault_secret" "acr_user_password" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = var.keyvault_ref_acr_user_password
  value        = var.acr_user_password
  key_vault_id = azurerm_key_vault.keyvault.id

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
