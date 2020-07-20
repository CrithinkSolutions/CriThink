using CriThink.Server.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Infrastructure
{
    public static class Bootstrapper
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<INewsSourceRepository, NewsSourceRepository>();
        }
    }
}
