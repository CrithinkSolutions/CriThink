output "ai_connection_string" {
  value     = azurerm_application_insights.ai_appsrv.connection_string
  sensitive = true
}
