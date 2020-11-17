using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Common.HttpRepository
{
    public static class Bootstrapper
    {
        public static void AddHttpRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRestRepository, RestRepository>();
        }
    }
}
