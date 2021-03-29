using CriThink.Common.HttpRepository;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.Identity;
using CriThink.Server.Infrastructure.Managers;
using CriThink.Server.Infrastructure.Repositories;
using CriThink.Server.Providers.DebunkNewsFetcher;
using CriThink.Server.Providers.EmailSender;
using CriThink.Server.Providers.NewsAnalyzer;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Infrastructure
{
    public static class Bootstrapper
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            // Data
            serviceCollection.AddSingleton<CriThinkRedisMultiplexer>();

            // HttpRepository
            serviceCollection.AddHttpRepository();

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
        }
    }
}
