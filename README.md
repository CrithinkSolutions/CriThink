# CriThink
CriThink is a mobile application able to identity fake news.
Developed by CriThink Solutions.

# Getting Started
## Server
### Run Server in Debug mode
The local development setting uses a local SQL Server instance.
It also uses the AspNet User Secret API. You can use this feature from the CLI or Visual Studio.
#### User Secret
##### Visual Studio
1. Right click on the project
2. Click on "Manage User Secrets"
3. This creates a new file
##### CLI
1. Execute the command: `dotnet user-secrets init`
2. Check if the csproj now contains a `<UserSecretsId>` tag
3. Execute the commands like the following to set a new secret value: `dotnet user-secrets set "Key:ApiKey" "12345"` or `dotnet user-secrets set "Key:ApiKey" "12345" --project "C:\apps\WebApp1\src\WebApp1"`

The directory where files are created is:
* Windows: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
* Unix: `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`

##### Required User Secrets
Insert the following keys and the desired values in the secret files created above:
* SQL Connection string: `"ConnectionStrings:CriThinkDbSqlConnection": "..."`
* JWT data:
    * `"Jwt-Audience": "DemoAudience"`
    * `"Jwt-Issuer": "DemoIssuer"`
    * `"Jwt-SecretKey": "secretkey_secretkey123!"`
* SendGrid data:
    * `"SendGridOptions-Key": "<key>"`


### Environments
Three environments have been configured:
* Debug
* Staging
* Production

The API service/environment can return the name of which environment in running on