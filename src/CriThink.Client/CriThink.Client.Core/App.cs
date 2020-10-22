using System;
using CriThink.Client.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            InitializeServiceCollection();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }

        private static void InitializeServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            MapServiceCollectionToMvx(serviceProvider, serviceCollection);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
        }

        private static void MapServiceCollectionToMvx(IServiceProvider serviceProvider,
            IServiceCollection serviceCollection)
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
