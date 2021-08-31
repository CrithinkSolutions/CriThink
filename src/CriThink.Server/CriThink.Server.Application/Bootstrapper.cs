using CriThink.Server.Application.DomainServices;
using CriThink.Server.Application.Facades;
using CriThink.Server.Application.Queries;
using CriThink.Server.Application.Services;
using CriThink.Server.Core.DomainServices;
using CriThink.Server.Providers.DebunkingNewsFetcher;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Application
{
    public static class Bootstrapper
    {
        public static void AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDebunkNewsFetcherProvider();

            // Domain Services
            serviceCollection.AddScoped<IDebunkingNewsPublisherService, DebunkingNewsPublisherService>();

            // Services
            serviceCollection.AddScoped<IUserAvatarService, UserAvatarService>();

            // Facades
            serviceCollection.AddScoped<IDebunkNewsFetcherFacade, DebunkNewsFetcherFacade>();

            // Queries
            serviceCollection.AddScoped<IStatisticsQueries, StatisticsQueries>();
            serviceCollection.AddScoped<IUserProfileQueries, UserProfileQueries>();
            serviceCollection.AddScoped<IIdentityQueries, IdentityQueries>();
            serviceCollection.AddScoped<INotificationQueries, NotificationQueries>();
            serviceCollection.AddScoped<IDebunkingNewsQueries, DebunkingNewsQueries>();
            serviceCollection.AddScoped<IDebunkingNewsPublisherQueries, DebunkingNewsPublisherQueries>();
            serviceCollection.AddScoped<IDebunkingNewsTriggerLogQueries, DebunkingNewsTriggerLogQueries>();
            serviceCollection.AddScoped<INewsSourceQueries, NewsSourceQueries>();
            serviceCollection.AddScoped<IUnknownNewsSourceQueries, UnknownNewsSourceQueries>();
        }
    }
}
