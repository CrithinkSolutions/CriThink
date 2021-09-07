using System.Collections.Generic;
using CriThink.Client.Core;
using CriThink.Client.Core.Converters;
using CriThink.Client.Core.Logging;
using CriThink.Client.Core.Platform;
using CriThink.Client.Droid.Bindings;
using CriThink.Client.Droid.PlatformDetails;
using CriThink.Client.Droid.Presenters;
using FFImageLoading;
using Google.Android.Material.BottomNavigation;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
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

namespace CriThink.Client.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override IDictionary<string, string> ViewNamespaceAbbreviations
        {
            get
            {
                var toReturn = base.ViewNamespaceAbbreviations;
                toReturn["CriThink"] = "CriThink.Client.Droid.Controls";
                return toReturn;
            }
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<BottomNavigationView>(
                MvxBottomNavigationItemChangedBinding.BindingKey, view => new MvxBottomNavigationItemChangedBinding(view));
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite(nameof(UsernameValueConverter), new UsernameValueConverter());
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);
            Mvx.IoCProvider.RegisterSingleton<IPlatformDetails>(new DroidDetails());
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
            => new ClearStackPresenter(AndroidViewAssemblies);

        #region Logging

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
                .WriteTo.AndroidLog(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj} ({SourceContext}) {Exception}")
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

        #endregion
    }
}