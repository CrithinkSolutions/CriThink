using System;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Infrastructure.Api;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.Identity;
using CriThink.Server.Infrastructure.Managers;
using CriThink.Server.Infrastructure.Repositories;
using CriThink.Server.Infrastructure.Services;
using CriThink.Server.Providers.DebunkingNewsFetcher;
using CriThink.Server.Providers.EmailSender;
using CriThink.Server.Providers.NewsAnalyzer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Refit;

namespace CriThink.Server.Infrastructure
{
    public static class Bootstrapper
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            // Data
            serviceCollection.AddSingleton<CriThinkRedisMultiplexer>();

            // HttpClient
            SetupHttpClient(serviceCollection);

            // Providers
            serviceCollection.AddNewsAnalyzerProvider();
            serviceCollection.AddDebunkNewsFetcherProvider();
            serviceCollection.AddEmailSenderProvider();

            // Managers
            serviceCollection.AddTransient<IJwtManager, JwtManager>();

            // Repositories
            serviceCollection.AddTransient<INewsSourceRepository, NewsSourceRepository>();
            serviceCollection.AddScoped<IRoleRepository, RoleRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();

            // Services
            serviceCollection.AddScoped<IFileService>(sp =>
            {
                var environment = sp.GetRequiredService<IHostEnvironment>();
                if (environment.IsDevelopment())
                    return new FileService(
                        sp.GetRequiredService<IWebHostEnvironment>(),
                        sp.GetRequiredService<IHttpContextAccessor>());

                return new S3Service(
                    sp.GetRequiredService<IConfiguration>(),
                    sp.GetService<ILogger<S3Service>>());
            });
        }

        private static void SetupHttpClient(IServiceCollection serviceCollection)
        {
            SetupExternalProvider<IFacebookApi>(serviceCollection, "FacebookApiUrl");
            SetupExternalProvider<IGoogleApi>(serviceCollection, "GoogleApiUrl");
        }

        private static void SetupExternalProvider<T>(IServiceCollection serviceCollection, string configKey)
            where T : class
        {
            serviceCollection.AddRefitClient<T>()
                .ConfigureHttpClient((sp, c) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    c.BaseAddress = new Uri(config[configKey]);
                })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(8),
                }));
        }
    }
}
