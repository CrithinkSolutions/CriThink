using CriThink.Server.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Application
{
    public static class Bootstrapper
    {
        public static void AddApplication(this IServiceCollection serviceCollection)
        {
            // Queries
            serviceCollection.AddScoped<IStatisticsQueries, StatisticsQueries>();
        }
    }
}
