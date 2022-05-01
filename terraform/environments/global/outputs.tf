output "global_acr_name" {
  value = azurerm_container_registry.acr.name
}

output "global_acr_login_server" {
  value = azurerm_container_registry.acr.login_server
}