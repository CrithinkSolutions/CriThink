using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using CriThink.Client.Droid.Controls;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;

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

            var email = FindViewById<TextInputEditText>(Resource.Id.txtInput_email);
            var username = FindViewById<TextInputEditText>(Resource.Id.txtInput_username);
            var password = FindViewById<TextInputEditText>(Resource.Id.txtInput_password);
            var repeatPassword = FindViewById<TextInputEditText>(Resource.Id.txtInput_repeatPassword);
            var btnSignUp = FindViewById<AppCompatButton>(Resource.Id.btn_signUp);
            var loader = FindViewById<LoaderView>(Resource.Id.layoutLoader);

            var set = CreateBindingSet();

            set.Bind(email).To(vm => vm.Email);
            set.Bind(username).To(vm => vm.Username);
            set.Bind(password).To(vm => vm.Password);
            set.Bind(repeatPassword).To(vm => vm.RepeatPassword);
            set.Bind(btnSignUp).To(vm => vm.SignUpCommand);
            set.Bind(loader).For(v => v.Visibility).To(vm => vm.IsLoading).WithConversion<MvxVisibilityValueConverter>();

            set.Apply();

            ReadIntentData();
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