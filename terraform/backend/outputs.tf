output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}
output "storage_account_name" {
  value = azurerm_storage_account.state.name
}
output "storage_container_name" {
  value = azurerm_storage_container.core-container.name
}