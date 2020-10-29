using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using CriThink.Client.Droid.SocialLogins;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    public abstract class BaseSocialLoginActivity<TViewModel> : MvxActivity<TViewModel> where TViewModel : class, IMvxViewModel
    {
        private const int RequestCode = 1;

        private SocialLoginProvider _socialLoginProvider;
        private GoogleSignInClient _signInClient;
        private GoogleSignInOptions _gso;
        private ICallbackManager _callbackManager;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (_socialLoginProvider == SocialLoginProvider.Google)
            {
                if (requestCode == RequestCode)
                {
                    var account = GoogleSignIn.GetSignedInAccountFromIntent(data);
                    var a = account.Result;
                }
            }
            else if (_socialLoginProvider == SocialLoginProvider.Facebook)
            {
                _callbackManager.OnActivityResult(requestCode, (int) resultCode, data);
            }
        }

        #region Facebook

        public void LoginUsingFacebook()
        {
            _socialLoginProvider = SocialLoginProvider.Facebook;

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
        }

        #endregion

        #region Google

        public void LoginUsingGoogle()
        {
            _socialLoginProvider = SocialLoginProvider.Google;

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