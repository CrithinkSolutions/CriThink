resource "azurerm_role_assignment" "app_service_acr_pull" {
  principal_id                     = azurerm_linux_web_app.appsrv.identity[0].principal_id
  role_definition_name             = "AcrPull"
  scope                            = var.acr_id #azurerm_container_registry.example.id TODO
  skip_service_principal_aad_check = false
}
