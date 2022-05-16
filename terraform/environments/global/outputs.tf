output "global_acr_name" {
  value = azurerm_container_registry.acr.name
}

output "global_acr_admin_username" {
  value = azurerm_container_registry.acr.admin_username
}

output "global_acr_admin_password" {
  value     = azurerm_container_registry.acr.admin_password
  sensitive = true
}

# output "global_acr_login_server" {
#   value = azurerm_container_registry.acr.login_server
# }
