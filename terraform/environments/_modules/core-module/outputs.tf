output "db_connection_string" {
  value     = "${azurerm_postgresql_server.pg_server.name}.postgres.database.azure.com;Port=5432;Username=${azurerm_postgresql_server.pg_server.administrator_login};Password=${azurerm_postgresql_server.pg_server.administrator_login_password};Database=${azurerm_postgresql_database.pg_database.name}"
  sensitive = true
}

output "app_service_dns_zone_name" {
  value = azurerm_dns_zone.app_service_dns_zone.name
}

output "redis_connection_string" {
  value     = azurerm_redis_cache.redis.primary_connection_string
  sensitive = true
}

output "keyvault_id" {
  value = azurerm_key_vault.keyvault.id
}
