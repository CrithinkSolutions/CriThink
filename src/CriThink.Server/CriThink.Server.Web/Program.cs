using System;
using Azure.Identity;
using CriThink.Server.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

WebApplication app = builder.Build();

startup.Configure(app);

app.Run();
