using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Web.BackgroundServices
{
    public abstract class SchedulerBackgroundService : BackgroundService
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;
        private Timer _timer;

        public SchedulerBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected Task ScheduleActivityAsync(TimeSpan interval, TimerCallback timerCallback, CancellationToken cancellationToken)
        {
            var nextRunTime = DateTime.UtcNow.Date.AddDays(1);
            var currentTime = DateTime.UtcNow;
            var firstInterval = nextRunTime.Subtract(currentTime);

            void SetupTimer()
            {
                var t1 = Task.Delay(firstInterval, cancellationToken);
                t1.Wait(cancellationToken);
                timerCallback.Invoke(null);
                _timer = new Timer(timerCallback, null, TimeSpan.Zero, interval);
            }

            Task.Run(SetupTimer, cancellationToken);
            return Task.CompletedTask;
        }

        protected void StopScheduledActivity<T>([CallerFilePath] string caller = "")
            where T : class
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger<T>>();

            logger?.LogInformation($"{caller} is shutting down");

            _timer?.Change(Timeout.Infinite, 0);

            logger?.LogInformation($"{caller} changed timer setup");
        }

        protected T ReadFromConfiguration<T>(string key)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetValue<T>(key);
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
