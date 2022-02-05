using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using CriThink.Client.Droid.Controls;
using FFImageLoading;
using FFImageLoading.Cross;
using FFImageLoading.Svg.Platform;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [IntentFilter(
        actions: new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
        DataSchemes = new[] { DeepLinkConstants.SchemaHTTP, DeepLinkConstants.SchemaHTTPS },
        DataHost = DeepLinkConstants.SchemaHost,
        DataPathPrefix = "/" + DeepLinkConstants.SchemaPrefixEmailConfirmation,
        AutoVerify = false)]
    [MvxActivityPresentation]
    [Activity(LaunchMode = LaunchMode.SingleTop, ClearTaskOnLaunch = true)]
    public class SignUpEmailView : MvxActivity<SignUpEmailViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.signupemail_view);
            MainApplication.SetGradientStatusBar(this);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtCaption = FindViewById<AppCompatTextView>(Resource.Id.txtCaption);
            var email = FindViewById<TextInputEditText>(Resource.Id.txtInput_email);
            var inputUsername = FindViewById<TextInputEditText>(Resource.Id.txtInput_username);
            var txtUsernameAvailable = FindViewById<AppCompatTextView>(Resource.Id.txtUsernameAvailable);
            var txtUsernameUnavailable = FindViewById<AppCompatTextView>(Resource.Id.txtUsernameUnavailable);
            var password = FindViewById<TextInputEditText>(Resource.Id.txtInput_password);
            var repeatPassword = FindViewById<TextInputEditText>(Resource.Id.txtInput_repeatPassword);
            var btnSignUp = FindViewById<AppCompatButton>(Resource.Id.btnSignUp);
            var loader = FindViewById<LoaderView>(Resource.Id.loader);
            var txtEditEmail = FindViewById<TextInputLayout>(Resource.Id.txtEditEmail);
            var tvHeaderEmail = FindViewById<AppCompatTextView>(Resource.Id.tv_header_email);
            var txtEditUsername = FindViewById<TextInputLayout>(Resource.Id.txtEditUsername);
            var tvHeaderUsername = FindViewById<AppCompatTextView>(Resource.Id.tv_header_username);
            var txtEditPassword = FindViewById<TextInputLayout>(Resource.Id.txtEditPassword);
            var tvHeaderPassord = FindViewById<AppCompatTextView>(Resource.Id.tv_header_password);
            var txtEditRepeatPassword = FindViewById<TextInputLayout>(Resource.Id.txtEditRepeatPassword);
            var tvHeaderRepeatPassword = FindViewById<TextView>(Resource.Id.tv_header_repeat_password);
            var imgAvatar = FindViewById<MvxSvgCachedImageView>(Resource.Id.imgAvatar);
            var imgEditAvatar = FindViewById<MvxCachedImageView>(Resource.Id.imgEditAvatar);

            ImageService.Instance
                .LoadCompiledResource("ic_logo.svg")
                .WithCustomDataResolver(new SvgDataResolver())
                .Transform(ViewModel.AvatarImageTransformations)
                .Into(imgAvatar);

            var set = CreateBindingSet();

            set.Bind(email).To(vm => vm.Email);

            set.Bind(inputUsername).To(vm => vm.Username);

            set.Bind(txtUsernameAvailable).To(vm => vm.UsernameAvailableText);
            set.Bind(txtUsernameUnavailable).To(vm => vm.UsernameUnvailableText);

            set.Bind(txtUsernameAvailable).For(v => v.Visibility).To(vm => vm.IsUsernameAvailable).WithConversion<MvxVisibilityValueConverter>();

            set.Bind(txtUsernameUnavailable).For(v => v.Visibility).To(vm => vm.IsUsernameUnavailable).WithConversion<MvxVisibilityValueConverter>();

            set.Bind(password).To(vm => vm.Password);
            set.Bind(repeatPassword).To(vm => vm.RepeatPassword);
            set.Bind(btnSignUp).For(v => v.BindClick()).To(vm => vm.SignUpCommand);
            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);
            set.Bind(imgAvatar).For(v => v.BindClick()).To(vm => vm.PickUpImageCommand);
            set.Bind(imgEditAvatar).For(v => v.BindClick()).To(vm => vm.PickUpImageCommand);

            set.Bind(imgAvatar).For(v => v.Transformations).To(vm => vm.AvatarImageTransformations);
            set.Bind(imgAvatar).For(v => v.ImagePath).To(vm => vm.CustomImagePath);

            set.Bind(btnSignUp).For(v => v.Text).ToLocalizationId("SignUp");
            set.Bind(tvHeaderEmail).For(v => v.Text).ToLocalizationId("EmailHint");
            set.Bind(tvHeaderUsername).For(v => v.Text).ToLocalizationId("UsernameHint");
            set.Bind(tvHeaderPassord).For(v => v.Text).ToLocalizationId("PasswordHint");
            set.Bind(tvHeaderRepeatPassword).For(v => v.Text).ToLocalizationId("RepeatPasswordHint");
            set.Bind(txtTitle).ToLocalizationId("SignUpEmailTitle");
            set.Bind(txtCaption).ToLocalizationId("SignUpEmailCaption");

            set.Apply();

            ReadIntentData();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

        private void ReadIntentData()
        {
            var code = Intent?.Data?.GetQueryParameter("code");
            var userId = Intent?.Data?.GetQueryParameter("userId");
            if (!string.IsNullOrWhiteSpace(code) &&
                !string.IsNullOrWhiteSpace(userId))
            {
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await ViewModel.ConfirmUserEmailAsync(userId, code).ConfigureAwait(true);
                });
            }
        }
    }
}