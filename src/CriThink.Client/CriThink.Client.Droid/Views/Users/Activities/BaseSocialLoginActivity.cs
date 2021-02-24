using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using CriThink.Client.Core.ViewModels.Users;
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
        private const int RequestCode = 1;

        private ExternalLoginProvider _externalLoginProvider;
        private GoogleSignInClient _signInClient;
        private GoogleSignInOptions _gso;
        private ICallbackManager _callbackManager;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (_externalLoginProvider == ExternalLoginProvider.Google)
            {
                if (requestCode == RequestCode)
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
                _callbackManager.OnActivityResult(requestCode, (int) resultCode, data);
            }
        }

        #region Facebook

        public void LoginUsingFacebook()
        {
            _externalLoginProvider = ExternalLoginProvider.Facebook;

            if (_callbackManager == null)
                InitFacebookCallbacks();

            if (AccessToken.CurrentAccessToken == null)
                LoginManager.Instance.LogInWithReadPermissions(this, new[] { "email", "public_profile" });
            else
                LoginManager.Instance.LogOut();
        }

        private void InitFacebookCallbacks()
        {
            _callbackManager = CallbackManagerFactory.Create();

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

            LoginManager.Instance.RegisterCallback(_callbackManager, loginCallback);
        }

        #endregion

        #region Google

        public async Task LoginUsingGoogle()
        {
            _externalLoginProvider = ExternalLoginProvider.Google;

            if (_gso == null || _signInClient == null)
                InitGoogleSignIn();

            var lastUser = GoogleSignIn.GetLastSignedInAccount(this);
            if (lastUser is null)
            {
                var intent = _signInClient.SignInIntent;
                StartActivityForResult(intent, RequestCode);
            }
            else
            {
                var googleAccount = await _signInClient.SilentSignInAsync();
                var token = googleAccount.IdToken;

                PerformLogin(token, ExternalLoginProvider.Google);
            }
        }

        private void InitGoogleSignIn()
        {
            var token = Resources.GetString(Resource.String.google_signin);

            _gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(token)
                .RequestEmail()
                .Build();

            _signInClient = GoogleSignIn.GetClient(this, _gso);
        }

        #endregion

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
                await ViewModel.ShowErrorMessage(ex, message);
            });
        }

        private void ShowErrorMessage(string message)
        {
            Task.Run(async () =>
            {
                await ViewModel.ShowErrorMessage(message);
            });
        }
    }
}