using System;
using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.SignUpView")]
    public class SignUpView : BaseSocialLoginActivity<SignUpViewModel>
    {
        private AppCompatButton _btnFb;
        private AppCompatButton _btnGoogle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.signup_view);

            _btnFb = FindViewById<AppCompatButton>(Resource.Id.btnFb);
            if (_btnFb != null)
                _btnFb.Click += BtnFacebook_Click;

            _btnGoogle = FindViewById<AppCompatButton>(Resource.Id.btnGoogle);
            if (_btnGoogle != null)
                _btnGoogle.Click += BtnGoogle_Click;

            var btnSignUpEmail = FindViewById<AppCompatButton>(Resource.Id.btnSignUp);
            var btnLogin = FindViewById<AppCompatButton>(Resource.Id.btnLogin);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtCaption = FindViewById<AppCompatTextView>(Resource.Id.txtCaption);
            var alreadyAccount = FindViewById<AppCompatTextView>(Resource.Id.alreadyAccount);

            var set = CreateBindingSet();

            set.Bind(txtTitle).ToLocalizationId("SignUpTitle");
            set.Bind(txtCaption).ToLocalizationId("SignUpCaption");
            set.Bind(btnSignUpEmail).To(vm => vm.NavigateToSignUpEmailCommand);
            set.Bind(btnSignUpEmail).For(v => v.Text).ToLocalizationId("SignUpEmail");
            set.Bind(btnLogin).To(vm => vm.NavigateToLoginCommand);
            set.Bind(btnLogin).For(v => v.Text).ToLocalizationId("Login");
            set.Bind(_btnGoogle).For(v => v.Text).ToLocalizationId("Google");
            set.Bind(_btnFb).For(v => v.Text).ToLocalizationId("Facebook");
            set.Bind(alreadyAccount).ToLocalizationId("AlreadyAccount");

            set.Apply();
        }

        private void BtnGoogle_Click(object sender, EventArgs e) => LoginUsingGoogle();

        private void BtnFacebook_Click(object sender, EventArgs e) => LoginUsingFacebook();

        #region IDispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_btnFb != null)
                _btnFb.Click -= BtnFacebook_Click;

            if (_btnGoogle != null)
                _btnGoogle.Click -= BtnGoogle_Click;
        }

        #endregion
    }
}