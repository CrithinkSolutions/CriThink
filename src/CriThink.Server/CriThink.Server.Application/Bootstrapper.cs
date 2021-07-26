using CriThink.Server.Application.Queries;
using CriThink.Server.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Application
{
    public static class Bootstrapper
    {
        public static void AddApplication(this IServiceCollection serviceCollection)
        {
            // Services
            serviceCollection.AddScoped<IUserAvatarService, UserAvatarService>();

            // Queries
            serviceCollection.AddScoped<IStatisticsQueries, StatisticsQueries>();
            serviceCollection.AddScoped<IUserProfileQueries, UserProfileQueries>();
        }
    }
}
