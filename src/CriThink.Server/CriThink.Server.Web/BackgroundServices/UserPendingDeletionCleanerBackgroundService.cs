using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.BackgroundServices
{
    public class UserPendingDeletionCleanerBackgroundService : SchedulerBackgroundService
    {
        public UserPendingDeletionCleanerBackgroundService(IServiceScopeFactory serviceScopeFactory)
           : base(serviceScopeFactory)
        { }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var interval = ReadFromConfiguration<TimeSpan>("BackgroundServices:DeletedUserCleaner");
            await ScheduleActivityAsync(interval, (o) => RemoveUsersPendingDeletion(), cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopScheduledActivity<UserPendingDeletionCleanerBackgroundService>();
            return Task.CompletedTask;
        }

        private async void RemoveUsersPendingDeletion()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
            var logger = scope.ServiceProvider.GetService<ILogger<UserPendingDeletionCleanerBackgroundService>>();

            logger?.LogInformation($"{nameof(UserPendingDeletionCleanerBackgroundService)} is starting");

            try
            {
                await identityService.CleanUpUsersScheduledDeletionAsync();
                logger?.LogInformation("User cleaned up");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error cleaning up users via scheduled task");
            }
        }
    }
}
