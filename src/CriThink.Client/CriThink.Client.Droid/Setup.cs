using System.Collections.Generic;
using CriThink.Client.Core;
using CriThink.Client.Core.Platform;
using CriThink.Client.Droid.Bindings;
using CriThink.Client.Droid.PlatformDetails;
using CriThink.Client.Droid.Presenters;
using Google.Android.Material.BottomNavigation;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;

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

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            Mvx.IoCProvider.RegisterSingleton<IPlatformDetails>(new DroidDetails());
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
            => new ClearStackPresenter(AndroidViewAssemblies);
    }
}