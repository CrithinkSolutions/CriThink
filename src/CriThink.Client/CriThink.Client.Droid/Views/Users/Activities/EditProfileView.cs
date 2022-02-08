using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using FFImageLoading.Cross;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.EditProfileView")]
    public class EditProfileView : MvxActivity<EditProfileViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.editprofile_view);

            MainApplication.SetGradientStatusBar(this);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var txtBioTitle = FindViewById<AppCompatTextView>(Resource.Id.txtBioTitle);
            var txtSocialTitle = FindViewById<AppCompatTextView>(Resource.Id.txtSocialTitle);

            var txtEditGivenName = FindViewById<TextInputLayout>(Resource.Id.txtEditGivenName);
            var txtEditFamilyName = FindViewById<TextInputLayout>(Resource.Id.txtEditFamilyName);
            var txtEditDescription = FindViewById<TextInputLayout>(Resource.Id.txtEditBio);
            var txtEditDoB = FindViewById<TextInputLayout>(Resource.Id.txtEditDoB);
            var txtEditGender = FindViewById<TextInputLayout>(Resource.Id.txtEditGender);
            var txtEditCountry = FindViewById<TextInputLayout>(Resource.Id.txtEditCountry);
            var txtEditTelegram = FindViewById<TextInputLayout>(Resource.Id.txtEditTelegram);
            var txtEditSkype = FindViewById<TextInputLayout>(Resource.Id.txtEditSkype);
            var txtEditTwitter = FindViewById<TextInputLayout>(Resource.Id.txtEditTwitter);
            var txtEditInstagram = FindViewById<TextInputLayout>(Resource.Id.txtEditInstagram);
            var txtEditFacebook = FindViewById<TextInputLayout>(Resource.Id.txtEditFacebook);
            var txtEditSnapchat = FindViewById<TextInputLayout>(Resource.Id.txtEditSnapchat);
            var txtEditYouTube = FindViewById<TextInputLayout>(Resource.Id.txtEditYouTube);
            var txtEditBlog = FindViewById<TextInputLayout>(Resource.Id.txtEditBlog);

            var tvHeaderGivenName = FindViewById<TextView>(Resource.Id.tv_header_givenName);
            var tvHeaderFamilyName = FindViewById<TextView>(Resource.Id.tv_header_familyName);
            var tvHeaderDescription = FindViewById<TextView>(Resource.Id.tv_header_bio);
            var tvHeaderDoB = FindViewById<TextView>(Resource.Id.tv_header_dob);
            var tvHeaderGender = FindViewById<TextView>(Resource.Id.tv_header_gender);
            var tvHeaderCountry = FindViewById<TextView>(Resource.Id.tv_header_country);
            var tvHeaderTelegram = FindViewById<TextView>(Resource.Id.tv_header_telegram);
            var tvHeaderSkype = FindViewById<TextView>(Resource.Id.tv_header_skype);
            var tvHeaderTwitter = FindViewById<TextView>(Resource.Id.tv_header_twitter);
            var tvHeaderInstagram = FindViewById<TextView>(Resource.Id.tv_header_instagram);
            var tvHeaderFacebook = FindViewById<TextView>(Resource.Id.tv_header_facebook);
            var tvHeaderSnapchat = FindViewById<TextView>(Resource.Id.tv_header_snapchat);
            var tvHeaderYouTube = FindViewById<TextView>(Resource.Id.tv_header_youtube);
            var tvHeaderBlog = FindViewById<TextView>(Resource.Id.tv_header_blog);

            var txtInputGivenName = FindViewById<TextInputEditText>(Resource.Id.txtInputGivenName);
            var txtInputFamilyName = FindViewById<TextInputEditText>(Resource.Id.txtInputFamilyName);
            var txtInputDescription = FindViewById<TextInputEditText>(Resource.Id.txtInputBio);
            var txtInputDob = FindViewById<TextInputEditText>(Resource.Id.txtInputDob);
            var txtInputGender = FindViewById<TextInputEditText>(Resource.Id.txtInputGender);
            var txtInputCountry = FindViewById<TextInputEditText>(Resource.Id.txtInputCountry);
            var txtInputTelegram = FindViewById<TextInputEditText>(Resource.Id.txtInputTelegram);
            var txtInputSkype = FindViewById<TextInputEditText>(Resource.Id.txtInputSkype);
            var txtInputTwitter = FindViewById<TextInputEditText>(Resource.Id.txtInputTwitter);
            var txtInputInstagram = FindViewById<TextInputEditText>(Resource.Id.txtInputInstagram);
            var txtInputFacebook = FindViewById<TextInputEditText>(Resource.Id.txtInputFacebook);
            var txtInputSnapchat = FindViewById<TextInputEditText>(Resource.Id.txtInputSnapchat);
            var txtInputYouTube = FindViewById<TextInputEditText>(Resource.Id.txtInputYouTube);
            var txtInputBlog = FindViewById<TextInputEditText>(Resource.Id.txtInputBlog);
            var imgAvatar = FindViewById<MvxSvgCachedImageView>(Resource.Id.imgAvatar);
            var imgEditAvatar = FindViewById<MvxCachedImageView>(Resource.Id.imgEditAvatar);

            var btnSave = FindViewById<AppCompatButton>(Resource.Id.btnSave);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var set = CreateBindingSet();

            set.Bind(txtInputGivenName).To(vm => vm.UserProfileViewModel.GivenName);
            set.Bind(txtInputFamilyName).To(vm => vm.UserProfileViewModel.FamilyName);
            set.Bind(txtInputDescription).To(vm => vm.UserProfileViewModel.Description);
            set.Bind(txtInputDob).To(vm => vm.UserProfileViewModel.DoBViewModel);
            set.Bind(txtInputGender).To(vm => vm.UserProfileViewModel.GenderViewModel.LocalizedEntry);
            set.Bind(txtInputCountry).To(vm => vm.UserProfileViewModel.Country);
            set.Bind(txtInputTelegram).To(vm => vm.UserProfileViewModel.Telegram);
            set.Bind(txtInputSkype).To(vm => vm.UserProfileViewModel.Skype);
            set.Bind(txtInputTwitter).To(vm => vm.UserProfileViewModel.Twitter);
            set.Bind(txtInputInstagram).To(vm => vm.UserProfileViewModel.Instagram);
            set.Bind(txtInputFacebook).To(vm => vm.UserProfileViewModel.Facebook);
            set.Bind(txtInputSnapchat).To(vm => vm.UserProfileViewModel.Snapchat);
            set.Bind(txtInputYouTube).To(vm => vm.UserProfileViewModel.YouTube);
            set.Bind(txtInputBlog).To(vm => vm.UserProfileViewModel.Blog);

            set.Bind(txtInputDob).For(v => v.BindClick()).To(vm => vm.SelectDobCommand);
            set.Bind(txtInputGender).For(v => v.BindClick()).To(vm => vm.SelectGenderCommand);

            set.Bind(txtEditDoB).For(v => v.BindClick()).To(vm => vm.SelectDobCommand);
            set.Bind(txtEditGender).For(v => v.BindClick()).To(vm => vm.SelectGenderCommand);

            set.Bind(imgAvatar).For(v => v.Transformations).To(vm => vm.LogoImageTransformations);
            set.Bind(imgAvatar).For(v => v.ImagePath).To(vm => vm.UserProfileViewModel.AvatarImagePath);

            set.Bind(imgAvatar).For(v => v.BindClick()).To(vm => vm.PickUpImageCommand);
            set.Bind(imgEditAvatar).For(v => v.BindClick()).To(vm => vm.PickUpImageCommand);

            set.Bind(btnSave).To(vm => vm.SaveCommand);

            set.Bind(txtBioTitle).ToLocalizationId("Bio");
            set.Bind(txtSocialTitle).ToLocalizationId("Social");
            set.Bind(txtTitle).ToLocalizationId("EditProfile");
            set.Bind(tvHeaderGivenName).For(v => v.Text).ToLocalizationId("GivenName");
            set.Bind(tvHeaderFamilyName).For(v => v.Text).ToLocalizationId("FamilyName");
            set.Bind(tvHeaderDescription).For(v => v.Text).ToLocalizationId("Description");
            set.Bind(tvHeaderDoB).For(v => v.Text).ToLocalizationId("DoB");
            set.Bind(tvHeaderGender).For(v => v.Text).ToLocalizationId("Gender");
            set.Bind(tvHeaderCountry).For(v => v.Text).ToLocalizationId("Country");
            set.Bind(tvHeaderTelegram).For(v => v.Text).ToLocalizationId("Telegram");
            set.Bind(tvHeaderSkype).For(v => v.Text).ToLocalizationId("Skype");
            set.Bind(tvHeaderTwitter).For(v => v.Text).ToLocalizationId("Twitter");
            set.Bind(tvHeaderInstagram).For(v => v.Text).ToLocalizationId("Instagram");
            set.Bind(tvHeaderFacebook).For(v => v.Text).ToLocalizationId("Facebook");
            set.Bind(tvHeaderSnapchat).For(v => v.Text).ToLocalizationId("Snapchat");
            set.Bind(tvHeaderYouTube).For(v => v.Text).ToLocalizationId("YouTube");
            set.Bind(tvHeaderBlog).For(v => v.Text).ToLocalizationId("Blog");
            set.Bind(btnSave).For(v => v.Text).ToLocalizationId("SaveProfile");

            set.Apply();

        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }
    }
}