output "terraform_stg_db_server_name" {
  value = azurerm_postgresql_server.pg-server.name
}

output "terraform_stg_db_name" {
  value = azurerm_postgresql_database.pg-database.name
}