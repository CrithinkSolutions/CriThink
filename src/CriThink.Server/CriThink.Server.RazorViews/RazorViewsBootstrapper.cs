using CriThink.Server.RazorViews.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.RazorViews
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class RazorViewsBootstrapper
    {
        public static void AddRazorViews(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
        }
    }
}
