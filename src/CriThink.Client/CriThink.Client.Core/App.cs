using System;
using System.IO;
using System.Reflection;
using Acr.UserDialogs;
using CriThink.Client.Core.Api;
using CriThink.Client.Core.Data.Settings;
using CriThink.Client.Core.Handlers;
using CriThink.Client.Core.Localization;
using CriThink.Client.Core.Repositories;
using CriThink.Client.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Plugin.ResxLocalization;
using MvvmCross.ViewModels;
using Refit;

namespace CriThink.Client.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            InitializeServiceCollection();

            InitializeInternalServices();

            RegisterCustomAppStart<AppStart>();
        }

        private static void InitializeInternalServices()
        {
            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);

            // Resx
            Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(new MvxResxTextProvider(AppResources.ResourceManager));

            // Repo
            Mvx.IoCProvider.RegisterType<ISQLiteRepository, SQLiteRepository>();
            Mvx.IoCProvider.RegisterType<SecureSettingsRepository>();
            Mvx.IoCProvider.RegisterType<ISettingsRepository, SettingsRepository>();
            Mvx.IoCProvider.RegisterType<IIdentityRepository, IdentityRepository>();

            // Services
            Mvx.IoCProvider.RegisterType<IGeolocationService, CacheGeolocationService>();
            Mvx.IoCProvider.RegisterType<GeolocationService>();
            Mvx.IoCProvider.RegisterType<IDebunkingNewsService, CacheDebunkingNewsService>();
            Mvx.IoCProvider.RegisterType<DebunkingNewsService>();
            Mvx.IoCProvider.RegisterType<IApplicationService, ApplicationService>();
            Mvx.IoCProvider.RegisterType<INewsSourceService, CacheNewsSourceService>();
            Mvx.IoCProvider.RegisterType<NewsSourceService>();
            Mvx.IoCProvider.RegisterType<IdentityService>();
            Mvx.IoCProvider.RegisterType<IIdentityService, CacheIdentityService>();
            Mvx.IoCProvider.RegisterType<IPlatformService, PlatformService>();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMemoryCache>(() => new MemoryCache(new MemoryCacheOptions
            {
                CompactionPercentage = 0.5
            }));
        }

        private static void InitializeServiceCollection()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("CriThink.Client.Core.appsettings.json");

            configBuilder.AddCommandLine(new[] { $"ContentRoot={Directory.GetDirectoryRoot("CriThink.Client.Droid")}" });
            configBuilder.AddJsonStream(stream);

            IConfigurationRoot configurationRoot = configBuilder.Build();
            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection, configurationRoot);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            MapServiceCollectionToMvx(serviceProvider, serviceCollection);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, IConfigurationRoot configurationRoot)
        {
            IMvxLog logger = null;

            if (Mvx.IoCProvider.CanResolve<IMvxLogProvider>())
            {
                logger = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<App>();
            }

            var baseApiUri = configurationRoot["BaseApiUri"];
            logger?.Info($"Starting app with uri: {baseApiUri}");

            if (string.IsNullOrWhiteSpace(baseApiUri))
            {
                var argumentException = new ArgumentException("The base uri is null");
                logger?.FatalException("Api Uri not found", argumentException);
                throw argumentException;
            }

            var debunkingNewsApiUri = configurationRoot["DebunkingNewsApiUri"];
            var identityApiUri = configurationRoot["IdentityApiUri"];
            var newsSourceApiUri = configurationRoot["NewsSourceApiUri"];

            serviceCollection.AddTransient<CriThinkApiHandler>();
            serviceCollection.AddTransient<AuthHeaderHandler>();

            serviceCollection.AddRefitClient<IIdentityApi>()
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(baseApiUri + identityApiUri);
                })
                .ConfigurePrimaryHttpMessageHandler<CriThinkApiHandler>();

            serviceCollection.AddRefitClient<IDebunkingNewsApi>()
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(baseApiUri + debunkingNewsApiUri);
                })
                .ConfigurePrimaryHttpMessageHandler<CriThinkApiHandler>()
                .AddHttpMessageHandler<AuthHeaderHandler>();

            serviceCollection.AddRefitClient<INewsSourceApi>()
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(baseApiUri + newsSourceApiUri);
                })
                .ConfigurePrimaryHttpMessageHandler<CriThinkApiHandler>()
                .AddHttpMessageHandler<AuthHeaderHandler>();
        }

        private static void MapServiceCollectionToMvx(IServiceProvider serviceProvider, IServiceCollection serviceCollection)
        {
            foreach (var serviceDescriptor in serviceCollection)
            {
                if (serviceDescriptor.ImplementationType != null)
                {
                    Mvx.IoCProvider.RegisterType(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType);
                }
                else if (serviceDescriptor.ImplementationFactory != null)
                {
                    var instance = serviceDescriptor.ImplementationFactory(serviceProvider);
                    Mvx.IoCProvider.RegisterSingleton(serviceDescriptor.ServiceType, instance);
                }
                else if (serviceDescriptor.ImplementationInstance != null)
                {
                    Mvx.IoCProvider.RegisterSingleton(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationInstance);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported registration type");
                }
            }
        }
    }
}
