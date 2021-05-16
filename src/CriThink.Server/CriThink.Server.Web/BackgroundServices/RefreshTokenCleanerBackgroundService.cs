using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.BackgroundServices
{
    public class RefreshTokenCleanerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public RefreshTokenCleanerBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            TimeSpan interval = GetFirstInterval();
            var nextRunTime = DateTime.UtcNow.Date.AddDays(1);
            var currentTime = DateTime.UtcNow;
            var firstInterval = nextRunTime.Subtract(currentTime);

            void SetupTimer()
            {
                var t1 = Task.Delay(firstInterval, cancellationToken);
                t1.Wait(cancellationToken);
                RemoveExpiredRefreshTokens(null);
                _timer = new Timer(RemoveExpiredRefreshTokens, null, TimeSpan.Zero, interval);
            }

            Task.Run(SetupTimer, cancellationToken);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger<RefreshTokenCleanerBackgroundService>>();

            logger?.LogInformation($"{nameof(RefreshTokenCleanerBackgroundService)} is shutting down");

            _timer?.Change(Timeout.Infinite, 0);

            logger?.LogInformation($"{nameof(RefreshTokenCleanerBackgroundService)} changed timer setup");

            return Task.CompletedTask;
        }

        private TimeSpan GetFirstInterval()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetValue<TimeSpan>("BackgroundServices:RefreshTokenCleaner");
        }

        private async void RemoveExpiredRefreshTokens(object state)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
            var logger = scope.ServiceProvider.GetService<ILogger<RefreshTokenCleanerBackgroundService>>();

            logger?.LogInformation($"{nameof(RefreshTokenCleanerBackgroundService)} is starting");

            try
            {
                await identityService.CleanUpExpiredRefreshTokens();
                logger?.LogInformation("Expired refresh token cleaned up");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error cleaning up expired refresh tokens via scheduled task");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
