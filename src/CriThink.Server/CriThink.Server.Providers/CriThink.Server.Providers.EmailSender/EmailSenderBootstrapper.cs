using CriThink.Server.Providers.EmailSender.Providers;
using CriThink.Server.Providers.EmailSender.Services;
using CriThink.Server.RazorViews;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CriThink.Server.Providers.EmailSender
{
    /// <summary>
    /// Bootstrapper to handle library initialization
    /// </summary>
    public static class EmailSenderBootstrapper
    {
        public static void AddEmailSenderProvider(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddRazorViews();
            serviceCollection.AddHttpContextAccessor();

            serviceCollection.AddTransient<IEmailSenderProvider>(sp =>
            {
                var environment = sp.GetRequiredService<IWebHostEnvironment>();
                if (environment.IsDevelopment())
                    return new LocalEmailSenderProvider();
                return new AwsEmailSenderProvider();
            });

            serviceCollection.AddTransient<IEmailSenderService, EmailSenderService>();
        }
    }
}
