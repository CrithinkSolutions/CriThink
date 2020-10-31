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
                    var account = GoogleSignIn.GetSignedInAccountFromIntent(data);
                    var a = account.Result;
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

            if (AccessToken.CurrentAccessToken != null)
                LoginManager.Instance.LogOut();
            else
                LoginManager.Instance.LogInWithReadPermissions(this, new[] { "email", "public_profile" });
        }

        private void InitFacebookCallbacks()
        {
            _callbackManager = CallbackManagerFactory.Create();

            var loginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    ViewModel.PerformLoginSignAsync(AccessToken.CurrentAccessToken.Token, ExternalLoginProvider.Facebook);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
        }

        #endregion

        #region Google

        public void LoginUsingGoogle()
        {
            _externalLoginProvider = ExternalLoginProvider.Google;

            if (_gso == null || _signInClient == null)
                InitGoogleSignIn();

            var lastUser = GoogleSignIn.GetLastSignedInAccount(this);
            if (lastUser == null)
            {
                var intent = _signInClient.SignInIntent;
                StartActivityForResult(intent, RequestCode);
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
    }
}