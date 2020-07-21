using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Infrastructure
{
    public static class Bootstrapper
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection, string redisConnectionString)
        {
            CriThinkRedisMultiplexer.SetRedisConfiguration(redisConnectionString);

            serviceCollection.AddTransient<INewsSourceRepository, NewsSourceRepository>();
        }
    }
}
