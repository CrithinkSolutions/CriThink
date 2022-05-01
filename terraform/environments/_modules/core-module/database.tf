resource "azurerm_postgresql_server" "pg-server" {
  name                         = var.db_server_name
  resource_group_name          = var.rg_name
  location                     = var.rg_location
  administrator_login          = var.db_admin_username
  administrator_login_password = var.db_admin_password
  sku_name                     = "B_Gen5_1"
  version                      = "11"
  ssl_enforcement_enabled      = true
  auto_grow_enabled            = false
  storage_mb                   = 5120

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}

resource "azurerm_postgresql_database" "pg-database" {
  name                = var.db_name
  resource_group_name = var.rg_name
  charset             = "UTF8"
  collation           = "English_United States.1252"
  server_name         = azurerm_postgresql_server.pg-server.name
}
