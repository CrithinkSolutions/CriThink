using System;
using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Platforms.Android.Presenters.Attributes;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.LoginView")]
    public class LoginView : BaseSocialLoginActivity<LoginViewModel>
    {
        private AppCompatButton _btnFb;
        private AppCompatButton _btnGoogle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.login_view);

            var btnLogin = FindViewById<AppCompatButton>(Resource.Id.btnLogin);
            var btnForgotPassword = FindViewById<AppCompatButton>(Resource.Id.btnForgotPassword);

            _btnFb = FindViewById<AppCompatButton>(Resource.Id.btnFb);
            if (_btnFb != null)
                _btnFb.Click += BtnFacebook_Click;

            _btnGoogle = FindViewById<AppCompatButton>(Resource.Id.btnGoogle);
            if (_btnGoogle != null)
                _btnGoogle.Click += BtnGoogle_Click;

            var btnJump = FindViewById<AppCompatButton>(Resource.Id.btnJump);

            var set = CreateBindingSet();

            set.Bind(btnLogin).To(vm => vm.LoginCommand);
            set.Bind(btnForgotPassword).To(vm => vm.ForgotPasswordCommand);
            set.Bind(btnJump).To(vm => vm.NavigateToHomeCommand);

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