using CriThink.Server.Providers.EmailSender.Services;
using CriThink.Server.RazorViews;
using Microsoft.Extensions.DependencyInjection;

namespace CriThink.Server.Providers.EmailSender
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class EmailSenderBootstrapper
    {
        public static void AddEmailSenderService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRazorViews();
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddTransient<IEmailSenderService, EmailSenderService>();
        }
    }
}
