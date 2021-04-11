using System;
using Amazon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                .ConfigureLogging((hostingContext, logging) =>
                {
                    if (hostingContext.HostingEnvironment.IsDevelopment()) return;

                    SetupAwsLogging(logging);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
                    {
                        if (hostingContext.HostingEnvironment.IsDevelopment()) return;

                        SetupAwsSecretManager(configBuilder);
                    });

                    webBuilder.UseStartup<Startup>();
                });

        private static void SetupAwsLogging(ILoggingBuilder logging)
        {
            logging.AddAWSProvider();
            logging.SetMinimumLevel(LogLevel.Information);
        }

        private static void SetupAwsSecretManager(IConfigurationBuilder configBuilder)
        {
#if DEBUG
            var chain = new Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain();

            var awsUser = Environment.GetEnvironm entVariable("AWS_PROFILE");

            if (chain.TryGetAWSCredentials(awsUser, out var credentials))
            {
                configBuilder.AddSecretsManager(credentials, RegionEndpoint.EUCentral1, options =>
                {
                    options.KeyGenerator = (entry, key) => key.Replace($"{entry.Name}:", "", StringComparison.InvariantCultureIgnoreCase);
                });
            }
#else
            var arnSecret = Environment.GetEnvironmentVariable("AWS_ARN_SECRET");

            configBuilder.AddSecretsManager(region: RegionEndpoint.EUCentral1, configurator: options =>
            {
                options.SecretFilter = entry => arnSecret == entry.ARN;
                options.KeyGenerator = (entry, key) => key.Replace($"{entry.Name}:", "", StringComparison.InvariantCultureIgnoreCase);
            });
#endif
        }
    }
}
