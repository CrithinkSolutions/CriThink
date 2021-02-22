using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Droid.Bindings;
using Google.Android.Material.BottomNavigation;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Xamarin.Facebook;

namespace CriThink.Client.Droid.Views
{
    [IntentFilter(
        actions: new[] { Intent.ActionSend },
        Categories = new[] { Intent.CategoryDefault },
        DataMimeType = "text/plain")]
    [MvxActivityPresentation]
    [Activity]
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

            var action = Intent?.Action;
            var type = Intent?.Type;

            if (action is Intent.ActionSend && type is "text/plain")
            {
                var text = Intent.GetStringExtra(Intent.ExtraText);
                ViewModel.NavigateToNewsCheckResultViewModel.Execute(text);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            Minimise();
        }

        private void Minimise()
        {
            var minimiseIntent = new Intent(Intent.ActionMain);
            minimiseIntent.AddCategory(Intent.CategoryHome);
            minimiseIntent.SetFlags(ActivityFlags.NewTask);
            StartActivity(minimiseIntent);
        }
    }
}