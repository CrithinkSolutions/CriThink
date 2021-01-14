using System;
using System.IO;
using System.Reflection;
using CriThink.Client.Core.Data.Settings;
using CriThink.Client.Core.Handlers;
using CriThink.Client.Core.Repositories;
using CriThink.Client.Core.Services;
using CriThink.Common.HttpRepository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Polly;

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
            // Repo
            Mvx.IoCProvider.RegisterType<IRestRepository, RestRepository>();
            Mvx.IoCProvider.RegisterType<SecureSettingsRepository>();
            Mvx.IoCProvider.RegisterType<ISettingsRepository, SettingsRepository>();
            Mvx.IoCProvider.RegisterType<IIdentityRepository, IdentityRepository>();

            // Services
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
            var baseApiUri = configurationRoot["BaseApiUri"];
            if (string.IsNullOrWhiteSpace(baseApiUri))
                throw new ArgumentException("The base uri is null");

            serviceCollection.AddTransient<CriThinkApiHandler>();
            serviceCollection
                .AddHttpClient("", httpClient =>
                {
                    httpClient.BaseAddress = new Uri(baseApiUri);
                })
                .ConfigurePrimaryHttpMessageHandler<CriThinkApiHandler>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                }));
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
