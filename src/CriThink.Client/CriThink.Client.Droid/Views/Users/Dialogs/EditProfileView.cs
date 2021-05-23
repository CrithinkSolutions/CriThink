using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using FFImageLoading.Cross;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.Material;
using MvvmCross.Platforms.Android.Presenters.Attributes;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(EditProfileView))]
    public class EditProfileView : MvxBottomSheetDialogFragment<EditProfileViewModel>
    {
        //private ProfileView BaseActivity => (ProfileView) Activity;

        public EditProfileView()
        { }

        protected EditProfileView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNormal, Resource.Style.AppBottomSheetDialogTheme);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = View.Inflate(Context, Resource.Layout.editprofile_view, null);

            //var toolbar = view.FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);

            //HasOptionsMenu = true;
            //BaseActivity.SetSupportActionBar(toolbar);
            //BaseActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //BaseActivity.SupportActionBar.SetDisplayShowTitleEnabled(false);

            var txtBioTitle = view.FindViewById<AppCompatTextView>(Resource.Id.txtBioTitle);
            var txtSocialTitle = view.FindViewById<AppCompatTextView>(Resource.Id.txtSocialTitle);

            var txtEditGivenName = view.FindViewById<TextInputLayout>(Resource.Id.txtEditGivenName);
            var txtEditFamilyName = view.FindViewById<TextInputLayout>(Resource.Id.txtEditFamilyName);
            var txtEditDescription = view.FindViewById<TextInputLayout>(Resource.Id.txtEditDescription);
            var txtEditDoB = view.FindViewById<TextInputLayout>(Resource.Id.txtEditDoB);
            var txtEditGender = view.FindViewById<TextInputLayout>(Resource.Id.txtEditGender);
            var txtEditCountry = view.FindViewById<TextInputLayout>(Resource.Id.txtEditCountry);
            var txtEditTelegram = view.FindViewById<TextInputLayout>(Resource.Id.txtEditTelegram);
            var txtEditSkype = view.FindViewById<TextInputLayout>(Resource.Id.txtEditSkype);
            var txtEditTwitter = view.FindViewById<TextInputLayout>(Resource.Id.txtEditTwitter);
            var txtEditInstagram = view.FindViewById<TextInputLayout>(Resource.Id.txtEditInstagram);
            var txtEditFacebook = view.FindViewById<TextInputLayout>(Resource.Id.txtEditFacebook);
            var txtEditSnapchat = view.FindViewById<TextInputLayout>(Resource.Id.txtEditSnapchat);
            var txtEditYouTube = view.FindViewById<TextInputLayout>(Resource.Id.txtEditYouTube);
            var txtEditBlog = view.FindViewById<TextInputLayout>(Resource.Id.txtEditBlog);

            var txtInputGivenName = view.FindViewById<TextInputEditText>(Resource.Id.txtInputGivenName);
            var txtInputFamilyName = view.FindViewById<TextInputEditText>(Resource.Id.txtInputFamilyName);
            var txtInputDescription = view.FindViewById<TextInputEditText>(Resource.Id.txtInputDescription);
            var txtInputDob = view.FindViewById<TextInputEditText>(Resource.Id.txtInputDob);
            var txtInputGender = view.FindViewById<TextInputEditText>(Resource.Id.txtInputGender);
            var txtInputCountry = view.FindViewById<TextInputEditText>(Resource.Id.txtInputCountry);
            var txtInputTelegram = view.FindViewById<TextInputEditText>(Resource.Id.txtInputTelegram);
            var txtInputSkype = view.FindViewById<TextInputEditText>(Resource.Id.txtInputSkype);
            var txtInputTwitter = view.FindViewById<TextInputEditText>(Resource.Id.txtInputTwitter);
            var txtInputInstagram = view.FindViewById<TextInputEditText>(Resource.Id.txtInputInstagram);
            var txtInputFacebook = view.FindViewById<TextInputEditText>(Resource.Id.txtInputFacebook);
            var txtInputSnapchat = view.FindViewById<TextInputEditText>(Resource.Id.txtInputSnapchat);
            var txtInputYouTube = view.FindViewById<TextInputEditText>(Resource.Id.txtInputYouTube);
            var txtInputBlog = view.FindViewById<TextInputEditText>(Resource.Id.txtInputBlog);
            var imgAvatar = view.FindViewById<MvxSvgCachedImageView>(Resource.Id.imgAvatar);

            var set = CreateBindingSet();

            set.Bind(txtInputGivenName).To(vm => vm.UserProfile.GivenName);
            set.Bind(txtInputFamilyName).To(vm => vm.UserProfile.FamilyName);
            set.Bind(txtInputDescription).To(vm => vm.UserProfile.Description);
            set.Bind(txtInputDob).To(vm => vm.UserProfile.DoB);
            set.Bind(txtInputGender).To(vm => vm.UserProfile.Gender);
            set.Bind(txtInputCountry).To(vm => vm.UserProfile.Country);
            set.Bind(txtInputTelegram).To(vm => vm.UserProfile.Telegram);
            set.Bind(txtInputSkype).To(vm => vm.UserProfile.Skype);
            set.Bind(txtInputTwitter).To(vm => vm.UserProfile.Twitter);
            set.Bind(txtInputInstagram).To(vm => vm.UserProfile.Instagram);
            set.Bind(txtInputFacebook).To(vm => vm.UserProfile.Facebook);
            set.Bind(txtInputSnapchat).To(vm => vm.UserProfile.Snapchat);
            set.Bind(txtInputYouTube).To(vm => vm.UserProfile.YouTube);
            set.Bind(txtInputBlog).To(vm => vm.UserProfile.Blog);

            set.Bind(imgAvatar).For(v => v.Transformations).To(vm => vm.LogoImageTransformations);
            set.Bind(imgAvatar).For(v => v.ImagePath).To(vm => vm.UserProfile.AvatarImagePath);

            set.Bind(txtBioTitle).ToLocalizationId("Bio");
            set.Bind(txtSocialTitle).ToLocalizationId("Social");

            set.Bind(txtEditGivenName).For(v => v.Hint).ToLocalizationId("GivenName");
            set.Bind(txtEditFamilyName).For(v => v.Hint).ToLocalizationId("FamilyName");
            set.Bind(txtEditDescription).For(v => v.Hint).ToLocalizationId("Description");
            set.Bind(txtEditDoB).For(v => v.Hint).ToLocalizationId("DoB");
            set.Bind(txtEditGender).For(v => v.Hint).ToLocalizationId("Gender");
            set.Bind(txtEditCountry).For(v => v.Hint).ToLocalizationId("Country");
            set.Bind(txtEditTelegram).For(v => v.Hint).ToLocalizationId("Telegram");
            set.Bind(txtEditSkype).For(v => v.Hint).ToLocalizationId("Skype");
            set.Bind(txtEditTwitter).For(v => v.Hint).ToLocalizationId("Twitter");
            set.Bind(txtEditInstagram).For(v => v.Hint).ToLocalizationId("Instagram");
            set.Bind(txtEditFacebook).For(v => v.Hint).ToLocalizationId("Facebook");
            set.Bind(txtEditSnapchat).For(v => v.Hint).ToLocalizationId("Snapchat");
            set.Bind(txtEditYouTube).For(v => v.Hint).ToLocalizationId("YouTube");
            set.Bind(txtEditBlog).For(v => v.Hint).ToLocalizationId("Blog");

            set.Apply();

            return view;
        }

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    Dismiss();
        //    return true;
        //}

        //public override Dialog OnCreateDialog(Bundle savedInstanceState)
        //{
        //    var dialog = base.OnCreateDialog(savedInstanceState);

        //    dialog.ShowEvent += (sender, args) =>
        //    {
        //        SetupFullHeight(dialog as BottomSheetDialog);
        //    };

        //    return dialog;
        //}
    }
}