using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using CriThink.Client.Core.ViewModels.Users;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

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
            MainApplication.SetGradientStatusBar(this);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtHello = FindViewById<AppCompatTextView>(Resource.Id.txtHello);
            var imgProfile = FindViewById<MvxSvgCachedImageView>(Resource.Id.imgProfile);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var txtRegistrationDate = FindViewById<AppCompatTextView>(Resource.Id.txtRegistrationDate);
            var txtAbout = FindViewById<AppCompatTextView>(Resource.Id.txtAbout);
            var txtAboutTitle = FindViewById<AppCompatTextView>(Resource.Id.txtAboutTitle);
            var txtCloseAccount = FindViewById<AppCompatButton>(Resource.Id.txtCloseAccount);
            var imgTelegram = FindViewById<MvxCachedImageView>(Resource.Id.imgTelegram);
            var imgSkype = FindViewById<MvxCachedImageView>(Resource.Id.imgSkype);
            var imgFacebook = FindViewById<MvxCachedImageView>(Resource.Id.imgFacebook);
            var imgInstagram = FindViewById<MvxCachedImageView>(Resource.Id.imgInstagram);
            var imgTwitter = FindViewById<MvxCachedImageView>(Resource.Id.imgTwitter);
            var imgSnapchat = FindViewById<MvxCachedImageView>(Resource.Id.imgSnapchat);
            var imgYoutube = FindViewById<MvxCachedImageView>(Resource.Id.imgYoutube);
            var imgBlog = FindViewById<MvxCachedImageView>(Resource.Id.imgBlog);

            var layoutName = FindViewById<ConstraintLayout>(Resource.Id.layoutName);
            var layoutCountry = FindViewById<ConstraintLayout>(Resource.Id.layoutCountry);
            var layoutDoB = FindViewById<ConstraintLayout>(Resource.Id.layoutDoB);
            var txtName = FindViewById<AppCompatTextView>(Resource.Id.txtName);
            var txtCountry = FindViewById<AppCompatTextView>(Resource.Id.txtCountry);
            var txtDoB = FindViewById<AppCompatTextView>(Resource.Id.txtDoB);
            var txtGender = FindViewById<AppCompatTextView>(Resource.Id.txtGender);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var set = CreateBindingSet();

            set.Bind(txtHello).To(vm => vm.HeaderText);
            set.Bind(imgProfile).For(v => v.Transformations).To(vm => vm.ProfileImageTransformations);
            set.Bind(imgProfile).For(v => v.ImagePath).To(vm => vm.UserProfileViewModel.AvatarImagePath);
            set.Bind(txtRegistrationDate).To(vm => vm.RegisteredOn);
            set.Bind(txtAbout).To(vm => vm.UserProfileViewModel.Description);

            set.Bind(txtName).To(vm => vm.UserFullNameFormat);
            set.Bind(txtCountry).To(vm => vm.UserCountryFormat);
            set.Bind(txtDoB).To(vm => vm.UserDoBFormat);
            set.Bind(txtGender).To(vm => vm.UserGenderFormat);

            set.Bind(imgTelegram).For(v => v.BindClick()).To(vm => vm.OpenTelegramCommand);
            set.Bind(imgSkype).For(v => v.BindClick()).To(vm => vm.OpenSkypeCommand);
            set.Bind(imgFacebook).For(v => v.BindClick()).To(vm => vm.OpenFacebookCommand);
            set.Bind(imgInstagram).For(v => v.BindClick()).To(vm => vm.OpenInstagramCommand);
            set.Bind(imgTwitter).For(v => v.BindClick()).To(vm => vm.OpenTwitterCommand);
            set.Bind(imgSnapchat).For(v => v.BindClick()).To(vm => vm.OpenSnapchatCommand);
            set.Bind(imgYoutube).For(v => v.BindClick()).To(vm => vm.OpenYoutubeCommand);
            set.Bind(imgBlog).For(v => v.BindClick()).To(vm => vm.OpenBlogCommand);
            set.Bind(txtCloseAccount).For(v => v.BindClick()).To(vm => vm.CloseAccountCommand);

            set.Bind(imgTelegram).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Telegram).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgSkype).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Skype).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgFacebook).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Facebook).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgFacebook).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Facebook).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgInstagram).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Instagram).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgTwitter).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Twitter).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgSnapchat).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Snapchat).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgYoutube).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.YouTube).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgBlog).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Blog).WithConversion<MvxVisibilityValueConverter>();

            set.Bind(layoutName).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.FullName).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(layoutCountry).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.Country).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(layoutDoB).For(v => v.Visibility).To(vm => vm.UserProfileViewModel.DoBViewModel.DateTime).WithConversion<MvxVisibilityValueConverter>();

            set.Bind(txtAboutTitle).ToLocalizationId("AboutTitle");
            set.Bind(txtTitle).ToLocalizationId("MyProfile");
            set.Bind(txtCloseAccount).For(v => v.Text).ToLocalizationId("CloseAccount");

            set.Apply();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
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