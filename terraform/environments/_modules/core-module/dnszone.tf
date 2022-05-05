resource "azurerm_dns_zone" "app_service_dns_zone" {
  name                = var.app_domain
  resource_group_name = var.rg_name

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
