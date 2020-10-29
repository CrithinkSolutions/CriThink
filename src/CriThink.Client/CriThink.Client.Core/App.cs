using System;
using System.IO;
using System.Reflection;
using CriThink.Client.Core.ViewModels;
using CriThink.Common.HttpRepository;
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

            Mvx.IoCProvider.RegisterType<IRestRepository, RestRepository>();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<WelcomeViewModel>();
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

            serviceCollection.AddHttpClient("", httpClient =>
            {
                httpClient.BaseAddress = new Uri(baseApiUri);
            })
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
