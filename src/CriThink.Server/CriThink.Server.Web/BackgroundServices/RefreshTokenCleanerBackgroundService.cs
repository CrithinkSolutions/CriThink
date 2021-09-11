using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.BackgroundServices
{
    public class RefreshTokenCleanerBackgroundService : SchedulerBackgroundService
    {
        public RefreshTokenCleanerBackgroundService(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        { }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var interval = ReadFromConfiguration<TimeSpan>("BackgroundServices:RefreshTokenCleaner");
            await ScheduleActivityAsync(interval, (o) => RemoveExpiredRefreshTokens(), cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopScheduledActivity<RefreshTokenCleanerBackgroundService>();
            return Task.CompletedTask;
        }

        private async void RemoveExpiredRefreshTokens()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var logger = scope.ServiceProvider.GetService<ILogger<RefreshTokenCleanerBackgroundService>>();

            logger?.LogInformation($"{nameof(RefreshTokenCleanerBackgroundService)} is starting");

            try
            {
                var command = new CleanUpExpiredRefreshTokensCommand();

                await mediator.Send(command);

                logger?.LogInformation("Expired refresh token cleaned up");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error cleaning up expired refresh tokens via scheduled task");
            }
        }
    }
}
