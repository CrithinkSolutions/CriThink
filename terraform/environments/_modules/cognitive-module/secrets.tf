resource "azurerm_key_vault_secret" "cognitive_key" {
  #   depends_on = [
  #     azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
  #     azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  #   ]

  name         = "AzureCognitive--KeyCredentials"
  value        = azurerm_cognitive_account.language.primary_access_key
  key_vault_id = var.keyvault_id
}
