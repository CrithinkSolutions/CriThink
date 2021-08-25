using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Droid.Bindings;
using FFImageLoading;
using Google.Android.Material.BottomNavigation;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Xamarin.Facebook;
using static CriThink.Client.Droid.Constants.FFImageConstants;
using CriThink.Common.Helpers;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

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
        public BottomNavigationView BottomNavigationView => FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

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
            Task.Run(async ()=> await SetBottomViewProfileIcon());
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

        private async Task SetBottomViewProfileIcon()
        {
            if (ViewModel.User?.AvatarPath.IsUrl() == true)
            {
                var states = new StateListDrawable();
                var unselectedBitmapDrawable = await ImageService.Instance
                    .LoadUrl(ViewModel.User?.AvatarPath)
                    .WithCache(FFImageLoading.Cache.CacheType.All)
                    .Transform(new ITransformation[] { new CropTransformation(1.1, 0, -0.2, 1, 1), new CircleTransformation()})
                    .AsBitmapDrawableAsync();
                var selectedBitmapDrawable = await ImageService.Instance
                    .LoadUrl(ViewModel.User?.AvatarPath)
                    .WithCache(FFImageLoading.Cache.CacheType.All)
                    .Transform(new ITransformation[] { new CropTransformation(1.1, 0, -0.2, 1, 1), new CircleTransformation(SelectedBorderSize, Resources.GetString(Resource.Color.menuTint)) })
                    .AsBitmapDrawableAsync();

                if (selectedBitmapDrawable != null
                    && unselectedBitmapDrawable != null)
                {
                    states.AddState(new int[] { Android.Resource.Attribute.StateChecked }, selectedBitmapDrawable);
                    states.AddState(new int[] { }, unselectedBitmapDrawable);
                    RunOnUiThread(() =>
                    {
                        BottomNavigationView.ItemIconTintList = null;
                        BottomNavigationView.Menu.FindItem(Resource.Id.page_4)?.SetIconTintMode(null);
                        BottomNavigationView.Menu.FindItem(Resource.Id.page_4)?.SetIcon(states);
                    });
                }
            }
        }
    }
}