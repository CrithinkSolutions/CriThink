resource "azurerm_virtual_network" "vnet" {
  name                = "stgvnet01"
  resource_group_name = var.rg_name
  location            = local.plan_region
  address_space       = ["10.0.0.0/16"]

  subnet {
    name           = "default"
    address_prefix = "10.0.0.0/24"
  }

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
