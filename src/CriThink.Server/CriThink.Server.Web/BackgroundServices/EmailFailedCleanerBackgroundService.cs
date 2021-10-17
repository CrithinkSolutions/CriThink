using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.BackgroundServices
{
    public class EmailFailedCleanerBackgroundService : SchedulerBackgroundService
    {
        public EmailFailedCleanerBackgroundService(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        { }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var interval = ReadFromConfiguration<TimeSpan>("BackgroundServices:EmailFailedCleaner");
            await ScheduleActivityAsync(interval, async (o) => await ResendEmailAsync(), cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopScheduledActivity<EmailFailedCleanerBackgroundService>();
            return Task.CompletedTask;
        }

        private async Task ResendEmailAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var logger = scope.ServiceProvider.GetService<ILogger<EmailFailedCleanerBackgroundService>>();

            logger?.LogInformation($"{nameof(EmailFailedCleanerBackgroundService)} is starting");

            try
            {
                var command = new CleanUpFailedEmailsCommand();

                await mediator.Send(command);

                logger?.LogInformation("Emails sent successfully");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error cleaning up failed emails via scheduled task");
            }
        }
    }
}
