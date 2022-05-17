# TODO: remove as soon as possible
locals {
  plan_region = "West Europe"
  acr_url     = "${var.acr_name}azurecr.io"
}

resource "azurerm_service_plan" "plan" {
  name                = var.appsrvpln_name
  resource_group_name = var.rg_name
  location            = local.plan_region
  os_type             = "Linux"
  sku_name            = "B1"

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}

resource "azurerm_linux_web_app" "appsrv" {
  depends_on = [
    azurerm_service_plan.plan
  ]

  name                = var.appsrv_name
  resource_group_name = var.rg_name
  location            = local.plan_region
  service_plan_id     = azurerm_service_plan.plan.id
  https_only          = true

  app_settings = {
    "DOCKER_REGISTRY_SERVER_URL"          = local.acr_url
    "DOCKER_REGISTRY_SERVER_USERNAME"     = var.acr_name
    "DOCKER_REGISTRY_SERVER_PASSWORD"     = "@Microsoft.KeyVault(SecretUri=https://${azurerm_key_vault.keyvault.name}.vault.azure.net/secrets/${var.keyvault_ref_acr_user_password}/)"
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "WEBSITE_HTTPLOGGING_RETENTION_DAYS"  = 2
    # "DOCKER_CUSTOM_IMAGE_NAME"            = "DOCKER|${var.acr_name}/crithink:latest" # TODO
  }

  site_config {}

  identity {
    type = "SystemAssigned"
  }

  tags = {
    application_name = var.tag_appname
    environment      = var.tag_environment
  }
}
