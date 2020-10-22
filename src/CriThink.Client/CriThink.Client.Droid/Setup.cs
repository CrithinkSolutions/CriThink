using CriThink.Client.Core;
using CriThink.Client.Droid.Bindings;
using Google.Android.Material.BottomNavigation;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Core;

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
    }
}