using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.ForgotPasswordView")]
    public class ForgotPasswordView : MvxActivity<ForgotPasswordViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.forgotpassword_view);

            var email = FindViewById<TextInputEditText>(Resource.Id.txtEdit_email);
            var btnSend = FindViewById<AppCompatButton>(Resource.Id.btnSend);
            var txtInputEmail = FindViewById<TextInputLayout>(Resource.Id.txtInput_email);

            var set = CreateBindingSet();

            set.Bind(email).To(vm => vm.EmailOrUsername);
            set.Bind(txtInputEmail).For(v => v.Hint).ToLocalizationId("InsertEmail");

            set.Bind(btnSend).To(vm => vm.SendRequestCommand);
            set.Bind(btnSend).ToLocalizationId("Send");

            set.Apply();
        }
    }
}