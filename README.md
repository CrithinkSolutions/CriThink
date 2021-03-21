# CriThink
CriThink is a platform able to identity trusted news sources.
Developed by CriThink Solutions.

## Build Status
Environment | Backend | Android App | iOS App | Demo
--- | --- | --- | --- | ---
Develop | [![CriThinkDemo](https://github.com/CrithinkSolutions/CriThink/actions/workflows/staging_server_publish.yml/badge.svg?branch=develop)](https://github.com/CrithinkSolutions/CriThink/actions/workflows/staging_server_publish.yml) | [![CriThink Android Beta](https://build.appcenter.ms/v0.1/apps/39416911-b2c3-47cf-a2af-37242e473532/branches/develop/badge)](https://appcenter.ms) | - | -
Main | [![CriThinkApp](https://github.com/CrithinkSolutions/CriThink/actions/workflows/main_server_publish.yml/badge.svg)](https://github.com/CrithinkSolutions/CriThink/actions/workflows/main_server_publish.yml) | - | - | -

<br>

# Getting Started
## Server
### Run Server in Debug mode
Before running the project, you need a PostgreSQL and Redis Cache instances. You can have your own on local machine or use the instances hosted on AWS for development purposes. Remmeber to apply migrations for the SQL database.

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
* PostgreSQL: `"ConnectionStrings:CriThinkDbPgSqlConnection": "<cs>"`
* Redis Cache: `"ConnectionStrings:CriThinkRedisCacheConnection": "<cs>"`
* JWT data:
    * `"Jwt-Audience": "<audience>"`
    * `"Jwt-Issuer": "<issuer>"`
    * `"Jwt-SecretKey": "<secretkey>"`
    * `"Jwt-ExpirationInHours": "<hours>"`
* Azure Cognitive Service:
    * `"Azure-Cognitive-KeyCredentials": "<credentials>"`
    * `"Azure-Cognitive-Endpoint": "<endpoint>"`
* Social networks:
    * `"FacebookApiKey": "<apiKey>"`
    * `"GoogleApiKey": "<apiKey>"`
* Users seed:
    * `"ServiceUser:SecurityStamp": "<stamp>"`
    * `"ServiceUser:PasswordHash": "<hash>"`
    * `"ServiceUser:EmailConfirmed": "<boolean>"`
    * `"ServiceUser:Email": "<email>"`
    * `"ServiceUser:NormalizedUserName": "<username>"`
    * `"ServiceUser:Id": "<id>"`
    * `"ServiceUser:ConcurrencyStamp": "<guid>"`
    * `"AdminRole:Name": "<name>"`
    * `"AdminRole:NormalizedName": "<name>"`
    * `"AdminRole:ConcurrencyStamp": "<guid>"`
* Headers:
    * `"CrossService:Header": "<header>"`
    * `"CrossService:DNews:Value": "<value>"`
    * `"CrossService:Scraper:Value": "<value>"`
    
##### User Secret for ReactJS
* Inside the `.Demo` project go into `ClientApp` folder and add a file called `.env.local`
* Write inside the file:
	* `REACT_APP_LOCALHOST=[Your localhost url]`

Note: You need to restart the development server after changing .env.local file.

#### Smtp4dev local SMTP server
In order to send easily mails even without an internet connection (and to not charge our AWS Subscription) [smtp4dev](https://github.com/rnwood/smtp4dev) was integrated.
1. Install smtp4dev for docker: `docker pull rnwood/smtp4dev:v3` (only Win and Linux)
2. Create a docker container: `docker run --name smtp4dev -p 3000:80 -p 2525:25 rnwood/smtp4dev:v3`
3. Start smtp4dev in docker: `docker start -a smtp4dev`

Browse to http://localhost:3000 with the GUI make sure if SMTP server is listening on port 2525

### Environments
Three environments have been configured:
* Debug
* Staging
* Production

### Service endpoints
* environment: returns the name of which environment is running
### Health endpoints
* health/redis: attempt a connection to the Redis cache
* health/postgresql: attempt a connection to the PostgreSQL database instance
* health/dbcontext: attempt a read-only access to the `DbContext` instance


### AWS Setup

#### ElasticBeanstalk
##### EC2 environment names:
* Staging
* Production

##### EC2 profiles:
* Staging: crithink-elasticbeanstalk-staging-role
* Production: crithink-elasticbeanstalk-production-role

Each profile has a specific custom policy to allow access only to the right secrets.

#### PostgreSQL
* Ask @Krusty93 for admin credentials (if needed)
* Set in the DB Securty Group (sg-de88c5a7) the access permission to the EC2 Security Group (sg-0121b6999052480c0)
* When a new instance is created, then go to "Edit" and enable the "Public accessibility"

#### Redis
* Set in the DB Securty Group (sg-de88c5a7) the access permission to the EC2 Security Group (sg-0121b6999052480c0)

#### Route53
* Create a Load Balancer
* Set an "A" type to the load balancer

#### HTTP redirect to HTTPS
* HTTPS forward to target group
* Target group forward to EC2 instance on port 80

## Mobile Apps
Developed using Xamarin and MvvmCross.

### Android
* Target SDK: 29
* Min SDK: 23
* Package name: `com.crithink.client.droid`

### iOS
* MinimumOSVersion: `11.2`
* Package name: `com.crithink.client.ios`
* Only iPhone
