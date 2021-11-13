using System;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Domain.Interfaces;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Api;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.Delegates;
using CriThink.Server.Infrastructure.Identity;
using CriThink.Server.Infrastructure.Managers;
using CriThink.Server.Infrastructure.Repositories;
using CriThink.Server.Infrastructure.SocialProviders;
using CriThink.Server.Providers.EmailSender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            serviceCollection.AddEmailSenderProvider();

            // Services
            serviceCollection.AddTransient<FacebookProvider>();
            serviceCollection.AddTransient<GoogleProvider>();
            serviceCollection.AddTransient<AppleProvider>();

            serviceCollection.AddTransient<ExternalLoginProviderResolver>(serviceProvider => externalProvider =>
            {
                return externalProvider switch
                {
                    ExternalLoginProvider.Facebook => serviceProvider.GetService<FacebookProvider>(),
                    ExternalLoginProvider.Google => serviceProvider.GetService<GoogleProvider>(),
                    ExternalLoginProvider.Apple => serviceProvider.GetService<AppleProvider>(),
                    _ => throw new NotSupportedException()
                };
            });

            // Managers
            serviceCollection.AddTransient<IJwtManager, JwtManager>();

            // Repositories
            serviceCollection.AddTransient<INewsSourceRepository, NewsSourceRepository>();
            serviceCollection.AddScoped<IRoleRepository, RoleRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IStatisticsRepository, StatisticsRepository>();
            serviceCollection.AddScoped<IDebunkingNewsRepository, DebunkingNewsRepository>();
            serviceCollection.AddScoped<INewsSourceCategoriesRepository, NewsSourceCategoriesRepository>();
            serviceCollection.AddScoped<INotificationRepository, NotificationRepository>();
            serviceCollection.AddScoped<INewsSourceQuestionRepository, NewsSourceQuestionRepository>();
            serviceCollection.AddScoped<IUnknownNewsSourceRepository, UnknownNewsSourceRepository>();
            serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            serviceCollection.AddScoped<IDebunkingNewsTriggerLogRepository, DebunkingNewsTriggerLogRepository>();
            serviceCollection.AddScoped<IEmailSendingFailureRepository, EmailSendingFailureRepository>();
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
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
        }
    }
}
