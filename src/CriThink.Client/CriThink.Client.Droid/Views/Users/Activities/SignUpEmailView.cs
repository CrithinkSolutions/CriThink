using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.SignUpEmailView")]
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

            var set = CreateBindingSet();

            set.Bind(email).To(vm => vm.Email);
            set.Bind(username).To(vm => vm.Username);
            set.Bind(password).To(vm => vm.Password);
            set.Bind(repeatPassword).To(vm => vm.RepeatPassword);
            set.Bind(btnSignUp).To(vm => vm.SignUpCommand);

            set.Apply();
        }
    }
}