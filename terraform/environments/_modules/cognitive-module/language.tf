# TODO: remove as soon as possible
locals {
  plan_region = "West Europe"
}

resource "azurerm_cognitive_account" "language" {
  name                = var.cognitive_account_name
  resource_group_name = var.rg_name
  location            = local.plan_region
  kind                = "TextAnalytics"

  sku_name = "F0"

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
