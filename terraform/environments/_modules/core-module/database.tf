resource "azurerm_postgresql_server" "pg_server" {

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

resource "azurerm_postgresql_database" "pg_database" {
  name                = var.db_name
  resource_group_name = var.rg_name
  charset             = "UTF8"
  collation           = "English_United States.1252"
  server_name         = azurerm_postgresql_server.pg_server.name
}

resource "azurerm_postgresql_firewall_rule" "firewall_exception" {
  depends_on = [
    azurerm_postgresql_database.pg_database
  ]

  count = length(azurerm_linux_web_app.appsrv.possible_outbound_ip_address_list)

  name                = "vnet_${count.index}"
  resource_group_name = var.rg_name
  server_name         = azurerm_postgresql_server.pg_server.name
  start_ip_address    = azurerm_linux_web_app.appsrv.possible_outbound_ip_address_list[count.index]
  end_ip_address      = azurerm_linux_web_app.appsrv.possible_outbound_ip_address_list[count.index]
}
