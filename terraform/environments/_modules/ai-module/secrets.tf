resource "azurerm_key_vault_secret" "cs_ai" {
  name         = "ConnectionStrings--ApplicationInsights"
  value        = azurerm_application_insights.ai_appsrv.connection_string
  key_vault_id = var.keyvault_id
}
