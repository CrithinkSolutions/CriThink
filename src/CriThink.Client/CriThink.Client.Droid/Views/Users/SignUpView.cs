using System;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.SocialLogins;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Name = "com.crithink.client.droid.SignUpView")]
    public class SignUpView : MvxActivity<SignUpViewModel>
    {
        ICallbackManager _callbackManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.signup_view);

            //Facebook
            var btnFb = FindViewById<AppCompatButton>(Resource.Id.btnFb);
            if (btnFb != null)
            {
                btnFb.Click += LoginUsingFacebook;
            }

            #region Facebook Login

            _callbackManager = CallbackManagerFactory.Create();

            var loginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {
                    //var token = AccessToken.CurrentAccessToken.Token;
                },
                HandleCancel = () =>
                {
                    //Handle Cancel
                },
                HandleError = loginError =>
                {
                    //Handle Error
                }
            };

            var loginStatusCallback = new LoginStatusCallback
            {
                HandleSuccess = loginResult =>
                {
                    //var token = AccessToken.CurrentAccessToken.Token;
                },
                HandleCancel = () =>
                {
                    //Handle Cancel
                },
                HandleError = loginError =>
                {
                    //Handle Error
                }
            };

            LoginManager.Instance.RegisterCallback(_callbackManager, loginCallback);
            LoginManager.Instance.RetrieveLoginStatus(this, loginStatusCallback);

            #endregion
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            _callbackManager.OnActivityResult(requestCode, (int) resultCode, data);
        }

        private void LoginUsingFacebook(object sender, EventArgs eventArgs)
        {
            if (AccessToken.CurrentAccessToken != null)
                LoginManager.Instance.LogOut();
            else
                LoginManager.Instance.LogInWithReadPermissions(this, new[] { "email", "public_profile" });
        }
    }
}