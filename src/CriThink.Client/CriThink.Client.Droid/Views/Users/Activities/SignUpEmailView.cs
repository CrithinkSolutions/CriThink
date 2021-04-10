using Android.App;
using Android.Content.PM;
using Android.OS;
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

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtCaption = FindViewById<AppCompatTextView>(Resource.Id.txtCaption);
            var email = FindViewById<TextInputEditText>(Resource.Id.txtInput_email);
            var username = FindViewById<TextInputEditText>(Resource.Id.txtInput_username);
            var password = FindViewById<TextInputEditText>(Resource.Id.txtInput_password);
            var repeatPassword = FindViewById<TextInputEditText>(Resource.Id.txtInput_repeatPassword);
            var btnSignUp = FindViewById<AppCompatButton>(Resource.Id.btnSignUp);
            var loader = FindViewById<LoaderView>(Resource.Id.loader);
            var txtEditEmail = FindViewById<TextInputLayout>(Resource.Id.txtEditEmail);
            var txtEditUsername = FindViewById<TextInputLayout>(Resource.Id.txtEditUsername);
            var txtEditPassword = FindViewById<TextInputLayout>(Resource.Id.txtEditPassword);
            var txtEditRepeatPassword = FindViewById<TextInputLayout>(Resource.Id.txtEditRepeatPassword);
            var imgAvatar = FindViewById<MvxSvgCachedImageView>(Resource.Id.imgAvatar);
            var imgEditAvatar = FindViewById<MvxCachedImageView>(Resource.Id.imgEditAvatar);

            ImageService.Instance
                .LoadCompiledResource("ic_logo.svg")
                .WithCustomDataResolver(new SvgDataResolver())
                .Transform(ViewModel.AvatarImageTransformations)
                .Into(imgAvatar);

            var set = CreateBindingSet();

            set.Bind(email).To(vm => vm.Email);
            set.Bind(username).To(vm => vm.Username);
            set.Bind(password).To(vm => vm.Password);
            set.Bind(repeatPassword).To(vm => vm.RepeatPassword);
            set.Bind(btnSignUp).For(v => v.BindClick()).To(vm => vm.SignUpCommand);
            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);
            set.Bind(imgAvatar).For(v => v.BindClick()).To(vm => vm.PickUpImageCommand);
            set.Bind(imgEditAvatar).For(v => v.BindClick()).To(vm => vm.PickUpImageCommand);

            set.Bind(imgAvatar).For(v => v.Transformations).To(vm => vm.AvatarImageTransformations);
            set.Bind(imgAvatar).For(v => v.ImagePath).To(vm => vm.CustomImagePath);

            set.Bind(btnSignUp).For(v => v.Text).ToLocalizationId("SignUp");
            set.Bind(txtEditEmail).For(v => v.Hint).ToLocalizationId("EmailHint");
            set.Bind(txtEditUsername).For(v => v.Hint).ToLocalizationId("UsernameHint");
            set.Bind(txtEditPassword).For(v => v.Hint).ToLocalizationId("PasswordHint");
            set.Bind(txtEditRepeatPassword).For(v => v.Hint).ToLocalizationId("RepeatPasswordHint");
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