using CriThink.Client.Core;
using CriThink.Client.Core.Logging;
using FFImageLoading;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Ios.Core;
#if APPCENTER
using SByteDev.Serilog.Sinks.AppCenter;
#endif
using Serilog;
using Serilog.Extensions.Logging;

#if APPCENTER
using Serilog.Events;
#endif
#if (LOGFILE)
using System.IO;
using Xamarin.Essentials;
#endif
namespace CriThink.Client.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Verbose()
#else
                .MinimumLevel.Information()
#endif
                .Enrich.FromLogContext()
                .WriteTo.NSLog(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj} ({SourceContext}) {Exception}")
#if APPCENTER
                .WriteTo.AppCenter(LogEventLevel.Information)
#endif
#if (LOGFILE)
                .WriteTo.File(
                    Path.Combine(FileSystem.AppDataDirectory, "Logs", "Logger.log"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj} ({SourceContext}) {Exception}{NewLine}"
                )
#endif
                .CreateLogger();

            ImageService.Instance.Config.Logger = new ImageLoaderLogger(Log.Logger);

            return new SerilogLoggerFactory();
        }
    }
}