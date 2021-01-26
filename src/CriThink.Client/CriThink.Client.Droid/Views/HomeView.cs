using Android.App;
using Android.OS;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Droid.Bindings;
using Google.Android.Material.BottomNavigation;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Xamarin.Facebook;

namespace CriThink.Client.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class HomeView : MvxActivity<HomeViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.home_view);

            var bottomView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            if (bundle == null)
                FacebookSdk.FullyInitialize();

            var set = CreateBindingSet();

            set.Bind(bottomView).For(MvxBottomNavigationItemChangedBinding.BindingKey).To(vm => vm.BottomNavigationItemSelectedCommand);

            set.Apply();

            ViewModel.BottomNavigationItemSelectedCommand.Execute(ViewModel.BottomViewTabs[0].TabId);
        }
    }
}