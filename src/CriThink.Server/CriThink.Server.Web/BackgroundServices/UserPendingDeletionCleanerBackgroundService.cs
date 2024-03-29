﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using MediatR;
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
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var logger = scope.ServiceProvider.GetService<ILogger<UserPendingDeletionCleanerBackgroundService>>();

            logger?.LogInformation($"{nameof(UserPendingDeletionCleanerBackgroundService)} is starting");

            try
            {
                var command = new CleanUpUsersScheduledDeletionCommand();

                await mediator.Send(command);

                logger?.LogInformation("User cleaned up");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error cleaning up users via scheduled task");
            }
        }
    }
}
