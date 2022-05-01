resource "azurerm_redis_cache" "redis" {
  name                          = var.redis_name
  resource_group_name           = var.rg_name
  location                      = var.rg_location
  capacity                      = 0
  family                        = "C"
  sku_name                      = "Basic"
  minimum_tls_version           = "1.2"
  public_network_access_enabled = true
  redis_version                 = 6

  redis_configuration {
    aof_backup_enabled = false
  }

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
