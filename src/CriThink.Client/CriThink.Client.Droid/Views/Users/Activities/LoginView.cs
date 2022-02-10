using System;
using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Controls;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

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

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            MainApplication.SetGradientStatusBar(this);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            var btnLogin = FindViewById<AppCompatButton>(Resource.Id.btnLogin);
            var btnForgotPassword = FindViewById<AppCompatButton>(Resource.Id.btnForgotPassword);
            var emailOrUsername = FindViewById<BindableEditText>(Resource.Id.txtEdit_emailOrUsername);
            var password = FindViewById<BindableEditText>(Resource.Id.txtEdit_password);
            var txtInputPassword = FindViewById<TextInputLayout>(Resource.Id.txtInput_password);
            var loader = FindViewById<LoaderView>(Resource.Id.loader);
            var txtInputEmail = FindViewById<TextInputLayout>(Resource.Id.txtInput_emailOrUsername);
            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtCaption = FindViewById<AppCompatTextView>(Resource.Id.txtCaption);
            var tvHeaderEmailOrUsername = FindViewById<TextView>(Resource.Id.tv_header_emailOrUsername);
            var tvHeaderPassword = FindViewById<TextView>(Resource.Id.tv_header_password);
            var txtOrSocial = FindViewById<AppCompatTextView>(Resource.Id.txtOrAccount);

            _btnFb = FindViewById<AppCompatButton>(Resource.Id.btnFb);
            if (_btnFb != null)
                _btnFb.Click += BtnFacebook_Click;

            _btnGoogle = FindViewById<AppCompatButton>(Resource.Id.btnGoogle);
            if (_btnGoogle != null)
                _btnGoogle.Click += BtnGoogle_Click;

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();

            set.Bind(emailOrUsername).To(vm => vm.EmailOrUsername);
            set.Bind(emailOrUsername).For(v => v.KeyCommand).To(vm => vm.LoginCommand);
            set.Bind(password).To(vm => vm.Password);
            set.Bind(password).For(v => v.KeyCommand).To(vm => vm.LoginCommand);
            set.Bind(txtOrSocial).ToLocalizationId("OrAccount");
            set.Bind(tvHeaderPassword).For(v => v.Text).ToLocalizationId("PasswordHint");
            set.Bind(tvHeaderEmailOrUsername).For(v => v.Text).ToLocalizationId("EmailHint");
            set.Bind(_btnFb).For(v => v.Text).ToLocalizationId("Facebook");
            set.Bind(_btnGoogle).For(v => v.Text).ToLocalizationId("Google");
            set.Bind(btnLogin).For(v => v.Text).ToLocalizationId("Login");
            set.Bind(btnLogin).To(vm => vm.LoginCommand);
            set.Bind(btnForgotPassword).For(v => v.Text).ToLocalizationId("ForgotPassword");
            set.Bind(btnForgotPassword).To(vm => vm.NavigateToForgotPasswordCommand);

            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);

            set.Bind(txtTitle).ToLocalizationId("WelcomeBack");
            set.Bind(txtCaption).ToLocalizationId("Pleasure");

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

        private void BtnGoogle_Click(object sender, EventArgs e) => LoginUsingGoogle();

        private void BtnFacebook_Click(object sender, EventArgs e) => LoginUsingFacebook();
    }
}