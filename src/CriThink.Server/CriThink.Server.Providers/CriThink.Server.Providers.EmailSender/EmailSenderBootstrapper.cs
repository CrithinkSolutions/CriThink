using CriThink.Server.Providers.EmailSender.Providers;
using CriThink.Server.Providers.EmailSender.Services;
using CriThink.Server.RazorViews;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Extensions.DependencyInjection;

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

            serviceCollection.AddSendGrid((sp, opt) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                opt.ApiKey = configuration["SendGridApiKey"];
            });

            serviceCollection.AddTransient<IEmailSenderProvider>(sp =>
            {
                var environment = sp.GetRequiredService<IWebHostEnvironment>();
                if (environment.IsDevelopment())
                    return new LocalEmailSenderProvider();

                return new SendGridEmailSenderProvider(
                    sp.GetRequiredService<ISendGridClient>(),
                    sp.GetService<ILogger<SendGridEmailSenderProvider>>());
            });

            serviceCollection.AddTransient<IEmailSenderService, EmailSenderService>();
        }
    }
}
