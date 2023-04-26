resource "azurerm_application_insights" "ai_appsrv" {
  name                = var.ai_name
  resource_group_name = var.rg_name
  location            = var.rg_location
  application_type    = "web"
  retention_in_days   = 30

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
