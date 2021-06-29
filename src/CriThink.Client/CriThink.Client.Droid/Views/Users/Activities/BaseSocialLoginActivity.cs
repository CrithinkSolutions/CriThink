using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Singletons;
using CriThink.Client.Droid.SocialLogins;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    public abstract class BaseSocialLoginActivity<TViewModel> : MvxActivity<TViewModel> where TViewModel : BaseSocialLoginViewModel, IMvxViewModel
    {
        private ExternalLoginProvider _externalLoginProvider;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (_externalLoginProvider == ExternalLoginProvider.Google)
            {
                if (requestCode == GoogleSingleton.ActivityResultCode)
                {
                    try
                    {
                        var account = GoogleSignIn.GetSignedInAccountFromIntent(data).Result;

                        if (!(account is GoogleSignInAccount googleAccount)) return;

                        var token = googleAccount.IdToken;

                        PerformLogin(token, ExternalLoginProvider.Google);
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(ex, "Login cancelled or an error occurred while logging with Google");
                    }
                }
            }
            else if (_externalLoginProvider == ExternalLoginProvider.Facebook)
            {
                FacebookSingleton.OnActivityResult(requestCode, resultCode, data);
            }
        }

        public void LoginUsingFacebook()
        {
            _externalLoginProvider = ExternalLoginProvider.Facebook;

            var loginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {
                    PerformLogin(AccessToken.CurrentAccessToken.Token, ExternalLoginProvider.Facebook);
                },
                HandleCancel = () =>
                {
                    //Handle Cancel
                },
                HandleError = loginError =>
                {
                    ShowErrorMessage("An error occurred when logging with Facebook");
                }
            };

            FacebookSingleton.Login(this, loginCallback);
        }

        public void LoginUsingGoogle()
        {
            _externalLoginProvider = ExternalLoginProvider.Google;

            var intent = GoogleSingleton.GetLoginIntent();
            StartActivityForResult(intent, GoogleSingleton.ActivityResultCode);
        }

        private void PerformLogin(string token, ExternalLoginProvider externalLoginProvider)
        {
            Task.Run(async () =>
            {
                await ViewModel.PerformLoginSignInAsync(token, externalLoginProvider);
            });
        }

        private void ShowErrorMessage(Exception ex, string message)
        {
            Task.Run(async () =>
            {
                await ViewModel.ShowErrorMessageAsync(ex, message);
            });
        }

        private void ShowErrorMessage(string message)
        {
            Task.Run(async () =>
            {
                await ViewModel.ShowErrorMessageAsync(message);
            });
        }
    }
}