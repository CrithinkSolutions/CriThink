## Azure Naming Conventions

### Resource names
#### Prefix
{environment}

#### Suffix
0x

#### Environments
- Global -> ""
- Staging -> "stg"
- Production -> "prod"
- Terraform -> "terraform"

#### Resource Types
- Resource Group -> crithink-{environment}
- Storage Account -> crithink{environment}stg{suffix}
- KeyVault -> {environment}keyvault{suffix}
- AppServicePlan -> {environment}plan{suffix}
- AppService -> {environment}appsrv{suffix}
- AppInsights -> {environment}ai{suffix}
- Functions -> {environment}functions{suffix}
- SQL (server) -> {environment}sql{suffix}
- SQL (database) -> {environment}db{suffix}
- Lock -> {blockedResourceName}-{level}
- ACR -> crithinkacr{suffix}
- SendGrid -> {environment}sendgrid{suffix}
- VNet -> {environment}vnet{suffix}

### Tags
application_name: CriThink
environment: Global/Staging/Production