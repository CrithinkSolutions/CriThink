using System;
using System.Collections.Generic;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.Services.Contracts;
using CriThink.Client.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Exception = System.Exception;

namespace CriThink.Client.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }
    }

    public class HostingAdapter : MvxSingleton<IMvxIoCProvider>, IMvxIoCProvider
    {
        private IServiceProvider ServiceProvider;
        private IServiceCollection ServiceCollection;

        public HostingAdapter()
        {
            //var a = Assembly.GetExecutingAssembly();
            //using var stream = a.GetManifestResourceStream("CriThink.Client.Core.appsettings.json");

            var host = Host
                .CreateDefaultBuilder()
                //.UseContentRoot("CriThink.Client.Core")
                //.ConfigureHostConfiguration(c =>
                //{

                //    // Tell the host configuration where to file the file (this is required for Xamarin apps)
                //    //c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                //    //read in the configuration file!
                //    c.AddJsonStream(stream);
                //})
                .ConfigureServices((context, serviceCollection) =>
                {
                    // Configure our local services and access the host configuration
                    ConfigureServices(context, serviceCollection);

                    ServiceCollection = serviceCollection;
                    ServiceProvider = ServiceCollection.BuildServiceProvider();
                })
                .Build();
        }


        private void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            //serviceCollection.AddTransient<IMvxLogProvider>(provider => new);
            serviceCollection.AddTransient<IIdentityService, IdentityService>();
        }

        protected override void Dispose(bool isDisposing)
        {
            ServiceProvider?.DisposeIfDisposable();
        }

        public bool CanResolve<T>() where T : class
        {
            try
            {
                ServiceProvider.GetRequiredService<T>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CanResolve(Type type)
        {
            try
            {
                ServiceProvider.GetRequiredService(type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public T Resolve<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        public object Resolve(Type type)
        {
            return ServiceProvider.GetRequiredService(type);
        }

        public bool TryResolve<T>(out T resolved) where T : class
        {
            try
            {
                resolved = ServiceProvider?.GetRequiredService<T>();
                return true;
            }
            catch (Exception ex)
            {
                resolved = default;
                return false;
            }
        }

        public bool TryResolve(Type type, out object resolved)
        {
            try
            {
                resolved = ServiceProvider?.GetRequiredService(type);
                return true;
            }
            catch (Exception ex)
            {
                resolved = default;
                return false;
            }
        }

        public T Create<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        public object Create(Type type)
        {
            return ServiceProvider.GetRequiredService(type);
        }

        public T GetSingleton<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        public object GetSingleton(Type type)
        {
            return ServiceProvider.GetRequiredService(type);
        }

        public void RegisterType<TFrom, TTo>() where TFrom : class where TTo : class, TFrom
        {
            ServiceCollection.AddTransient<TFrom, TTo>();
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class
        {
            ServiceCollection.AddTransient(provider => constructor.Invoke());
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterType(Type t, Func<object> constructor)
        {
            ServiceCollection.AddTransient(t, provider => constructor.Invoke());
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterType(Type tFrom, Type tTo)
        {
            ServiceCollection.AddTransient(tFrom, tTo);
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterSingleton<TInterface>(TInterface theObject) where TInterface : class
        {
            ServiceCollection.AddSingleton<TInterface>(theObject);
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterSingleton(Type tInterface, object theObject)
        {
            ServiceCollection.AddSingleton(tInterface, theObject);
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor) where TInterface : class
        {
            ServiceCollection.AddSingleton<TInterface>(provider => theConstructor.Invoke());
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            ServiceCollection.AddSingleton(tInterface, provider => theConstructor.Invoke());
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public T IoCConstruct<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        public T IoCConstruct<T>(IDictionary<string, object> arguments) where T : class
        {
            throw new NotImplementedException();
        }

        public T IoCConstruct<T>(object arguments) where T : class
        {
            throw new NotImplementedException();
        }

        public T IoCConstruct<T>(params object[] arguments) where T : class
        {
            throw new NotImplementedException();
        }

        public object IoCConstruct(Type type)
        {
            return ServiceProvider.GetRequiredService(type);
        }

        public object IoCConstruct(Type type, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public object IoCConstruct(Type type, object arguments)
        {
            throw new NotImplementedException();
        }

        public object IoCConstruct(Type type, params object[] arguments)
        {
            throw new NotImplementedException();
        }

        public void CallbackWhenRegistered<T>(Action action)
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void CallbackWhenRegistered(Type type, Action action)
        {
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public IMvxIoCProvider CreateChildContainer()
        {
            return this;
        }
    }
}
