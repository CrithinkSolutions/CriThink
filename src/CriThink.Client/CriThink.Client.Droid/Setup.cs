using System;
using CriThink.Client.Core;
using CriThink.Client.Droid.Bindings;
using Google.Android.Material.BottomNavigation;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

namespace CriThink.Client.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<BottomNavigationView>(
                MvxBottomNavigationItemChangedBinding.BindingKey, view => new MvxBottomNavigationItemChangedBinding(view));
        }

        protected override void InitializeLifetimeMonitor()
        {
            var lifetimeMonitor = CreateLifetimeMonitor();

            Mvx.IoCProvider.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(lifetimeMonitor);
        }

        protected override IMvxSettings InitializeSettings()
        {
            var mvxSettings = new MvxSettings();
            Mvx.IoCProvider.RegisterSingleton<IMvxSettings>(mvxSettings);
            return Mvx.IoCProvider.GetSingleton<IMvxSettings>();
        }

        protected override MvxAndroidLifetimeMonitor CreateLifetimeMonitor()
        {
            return new MvxAndroidLifetimeMonitor();
        }

        protected override IMvxNameMapping InitializeViewModelTypeFinder()
        {
            var viewModelByNameLookup = CreateViewModelByNameLookup();
            var viewModelByNameRegistry = CreateViewModelByNameRegistry();

            var viewModelAssemblies = GetViewModelAssemblies();
            foreach (var assembly in viewModelAssemblies)
            {
                viewModelByNameRegistry.AddAll(assembly);
            }

            var nameMappingStrategy = CreateViewToViewModelNaming();
            Mvx.IoCProvider.RegisterSingleton<IMvxViewModelTypeFinder>(new MvxViewModelViewTypeFinder(viewModelByNameLookup, nameMappingStrategy));
            Mvx.IoCProvider.RegisterSingleton(nameMappingStrategy);
            return nameMappingStrategy;
        }

        protected override IMvxLogProvider CreateLogProvider()
        {
            return new AppCenterLogProvider();
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var hostingAdapter = new HostingAdapter();
            return hostingAdapter;
        }

        public class AppCenterLogProvider : IMvxLogProvider
        {
            private readonly AppCenterLogger _logger;

            public AppCenterLogProvider()
            {
                _logger = new AppCenterLogger();
            }

            public IMvxLog GetLogFor(Type type)
            {
                return _logger;
            }

            public IMvxLog GetLogFor<T>()
            {
                return _logger;
            }

            public IMvxLog GetLogFor(string name)
            {
                return _logger;
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }
        }

        public class AppCenterLogger : IMvxLog
        {
            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
            {
                return true;
            }

            public bool IsLogLevelEnabled(MvxLogLevel logLevel)
            {
                return true;
            }
        }
    }
}