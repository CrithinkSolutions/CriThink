using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace CriThink.Client.Droid.Views.Users
{
    [MvxFragmentPresentation(typeof(WelcomeViewModel), Resource.Id.pager)]
    [Register(ViewConstants.Namespace + ".users." + nameof(SignUpView))]
    public class SignUpView : MvxFragment<SignUpViewModel>
    {
        public WelcomeView BaseActivity => (WelcomeView) Activity;

        private AppCompatButton _btnFb;
        private AppCompatButton _btnGoogle;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.signup_view, null);

            _btnFb = view.FindViewById<AppCompatButton>(Resource.Id.btnFb);
            if (_btnFb != null)
                _btnFb.Click += BtnFacebook_Click;

            _btnGoogle = view.FindViewById<AppCompatButton>(Resource.Id.btnGoogle);
            if (_btnGoogle != null)
                _btnGoogle.Click += BtnGoogle_Click;

            var btnSignUpEmail = view.FindViewById<AppCompatButton>(Resource.Id.btnSignUp);
            var btnLogin = view.FindViewById<AppCompatButton>(Resource.Id.btnLogin);

            var set = CreateBindingSet();

            set.Bind(btnSignUpEmail).To(vm => vm.NavigateToSignUpEmailCommand);
            set.Bind(btnLogin).To(vm => vm.NavigateToLoginCommand);

            set.Apply();

            return view;
        }

        private void BtnGoogle_Click(object sender, EventArgs e) => BaseActivity.LoginUsingGoogle();

        private void BtnFacebook_Click(object sender, EventArgs e) => BaseActivity.LoginUsingFacebook();

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