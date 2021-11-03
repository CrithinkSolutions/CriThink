using CriThink.Server.Application.DomainServices;
using CriThink.Server.Application.Facades;
using CriThink.Server.Application.Queries;
using CriThink.Server.Domain.DomainServices;
using CriThink.Server.Infrastructure.DomainServices;
using CriThink.Server.Providers.DebunkingNewsFetcher;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application
{
    public static class Bootstrapper
    {
        public static void AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDebunkNewsFetcherProvider();

            // Domain Services
            serviceCollection.AddScoped<IDebunkingNewsPublisherService, DebunkingNewsPublisherService>();

            // Facades
            serviceCollection.AddScoped<IDebunkNewsFetcherFacade, DebunkNewsFetcherFacade>();

            // Queries
            serviceCollection.AddScoped<IStatisticsQueries, StatisticsQueries>();
            serviceCollection.AddScoped<IIdentityQueries, IdentityQueries>();
            serviceCollection.AddScoped<INotificationQueries, NotificationQueries>();
            serviceCollection.AddScoped<IDebunkingNewsQueries, DebunkingNewsQueries>();
            serviceCollection.AddScoped<IDebunkingNewsPublisherQueries, DebunkingNewsPublisherQueries>();
            serviceCollection.AddScoped<IDebunkingNewsTriggerLogQueries, DebunkingNewsTriggerLogQueries>();
            serviceCollection.AddScoped<INewsSourceQueries, NewsSourceQueries>();
            serviceCollection.AddScoped<IUnknownNewsSourceQueries, UnknownNewsSourceQueries>();

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
    }
}
