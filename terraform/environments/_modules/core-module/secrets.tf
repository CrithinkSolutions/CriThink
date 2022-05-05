resource "azurerm_key_vault_secret" "acr_user_password" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = var.keyvault_ref_acr_user_password
  value        = var.acr_user_password
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "jwt_secret_key" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "Jwt--SecretKey"
  value        = var.jwt_secret_key
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "cross_service_scraper_value" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "CrossService--Scraper--Value"
  value        = var.cross_service_scraper_value
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "cross_service_dnews_value" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "CrossService--DNews--Value"
  value        = var.cross_service_dnews_value
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "cs_redis" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "ConnectionStrings--CriThinkRedisCacheConnection"
  value        = azurerm_redis_cache.redis.primary_connection_string
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "cs_pgsql" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name = "ConnectionStrings--CriThinkDbPgSqlConnection"
  // Host=crithink-demosql01.postgres.database.azure.com;Port=5432;Username=boss@crithink-demosql01;Password=king2Pac!;Database=demodb01
  value        = "Host=${azurerm_postgresql_server.pg_server.name}.postgres.database.azure.com;Port=5432;Username=${azurerm_postgresql_server.pg_server.administrator_login}@${azurerm_postgresql_server.pg_server.name};Password=${azurerm_postgresql_server.pg_server.administrator_login_password};Database=${azurerm_postgresql_database.pg_database.name}"
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "cs_internal_storage" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "ConnectionStrings--InternalStorageAccount"
  value        = azurerm_storage_account.internal_storage.primary_connection_string
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "authentication_google_client_secret" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "Authentication--Google--ClientSecret"
  value        = var.authentication_google_client_secret
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "authentication_google_client_id" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "Authentication--Google--ClientId"
  value        = var.authentication_google_client_id
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "authentication_facebook_client_secret" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "Authentication--Facebook--ClientSecret"
  value        = var.authentication_facebook_client_secret
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "authentication_facebook_client_id" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "Authentication--Facebook--ClientId"
  value        = var.authentication_facebook_client_id
  key_vault_id = azurerm_key_vault.keyvault.id
}

resource "azurerm_key_vault_secret" "sendgrid_api_key" {
  depends_on = [
    azurerm_key_vault_access_policy.keyvault-app-accesspolicy,
    azurerm_key_vault_access_policy.keyvault-appsrv-accesspolicy
  ]

  name         = "SendGridApiKey"
  value        = var.sendgrid_api_key
  key_vault_id = azurerm_key_vault.keyvault.id
}
