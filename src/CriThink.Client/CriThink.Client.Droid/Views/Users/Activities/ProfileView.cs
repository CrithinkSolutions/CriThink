using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using FFImageLoading.Cross;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.LoginView")]
    public class ProfileView : MvxActivity<ProfileViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.profile_view);

            var txtHello = FindViewById<AppCompatTextView>(Resource.Id.txtHello);
            var imgProfile = FindViewById<MvxSvgCachedImageView>(Resource.Id.imgProfile);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var set = CreateBindingSet();

            set.Bind(txtHello).To(vm => vm.HeaderText);
            set.Bind(imgProfile).For(v => v.Transformations).To(vm => vm.ProfileImageTransformations);
            set.Bind(imgProfile).For(v => v.ImagePath).To(vm => vm.AvatarImagePath);

            set.Apply();
        }

        public override bool OnCreateOptionsMenu(IMenu? menu)
        {
            MenuInflater.Inflate(Resource.Menu.edit_actionbar, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.actionEdit)
                ViewModel.NavigateToEditCommand.Execute();

            return base.OnOptionsItemSelected(item);
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }
    }
}