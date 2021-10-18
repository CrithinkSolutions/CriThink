using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.CoordinatorLayout.Widget;
using CriThink.Client.Core.ViewModels.Users;
using FFImageLoading.Cross;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.Material;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(EditProfileView))]
    public class EditProfileView : MvxBottomSheetDialogFragment<EditProfileViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(StyleNormal, Resource.Style.AppBottomSheetDialogTheme);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = View.Inflate(Context, Resource.Layout.editprofile_view, null);

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
            var imgEditAvatar = view.FindViewById<MvxCachedImageView>(Resource.Id.imgEditAvatar);

            var btnSave = view.FindViewById<AppCompatButton>(Resource.Id.btnSave);

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
            set.Bind(btnSave).For(v => v.Text).ToLocalizationId("SaveProfile");

            set.Apply();
            
            return view;
        }
    }
    

}