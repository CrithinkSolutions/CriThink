using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.Core.Content.Resources;
using AndroidX.Core.Graphics.Drawable;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Droid.Bindings;
using CriThink.Client.Droid.Extensions;
using FFImageLoading;
using FFImageLoading.Work;
using Google.Android.Material.BottomNavigation;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Xamarin.Facebook;
using CriThink.Client.Core.Extensions;
using static Android.Graphics.Bitmap;
using static Android.Graphics.PorterDuff;
using System.Threading.Tasks;

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
                var color = Color.ParseColor(Resources.GetString(Resource.Color.menuTint));
                var states = new StateListDrawable();
                var bitmapDrawable = await ImageService.Instance
                    .LoadUrl(ViewModel.User.AvatarPath)
                    .WithCache(FFImageLoading.Cache.CacheType.All)
                    .AsBitmapDrawableAsync();

                if (bitmapDrawable != null
                    && bitmapDrawable.Bitmap != null)
                {
                    var unselectedBitmapDrawable = new BitmapDrawable(Resources, bitmapDrawable.Bitmap.RoundedCornerBitmap(80, 2, Color.Transparent));
                    var selectedBitmapDrawable = new BitmapDrawable(Resources, bitmapDrawable.Bitmap.RoundedCornerBitmap(80, 2, color));
                    states.AddState(new int[] { Android.Resource.Attribute.StateChecked }, selectedBitmapDrawable);
                    states.AddState(new int[] { }, unselectedBitmapDrawable);
                    BottomNavigationView.ItemIconTintList = null;
                    BottomNavigationView.Menu.FindItem(Resource.Id.page_4)?.SetIconTintMode(null);
                    RunOnUiThread(() => BottomNavigationView.Menu.FindItem(Resource.Id.page_4)?.SetIcon(states));
                }
            }
        }
    }
}