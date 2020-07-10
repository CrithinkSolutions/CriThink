using System;
using Amazon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable

namespace CriThink.Server.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
                    {
                        if (hostingContext.HostingEnvironment.IsDevelopment()) return;

                        SetupAWSSecretManager(configBuilder);
                    });

                    webBuilder.UseStartup<Startup>();
                });

        private static void SetupAWSSecretManager(IConfigurationBuilder configBuilder)
        {
            var awsUser = Environment.GetEnvironmentVariable("AWS_USER");

            var chain = new Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain();

            if (chain.TryGetAWSCredentials(awsUser, out var credentials))
            {
                configBuilder.AddSecretsManager(credentials, RegionEndpoint.EUWest3, options =>
                {
                    options.KeyGenerator = (entry, key) => key.Replace($"{entry.Name}:", "", StringComparison.InvariantCultureIgnoreCase);
                });
            }
        }
    }
}
