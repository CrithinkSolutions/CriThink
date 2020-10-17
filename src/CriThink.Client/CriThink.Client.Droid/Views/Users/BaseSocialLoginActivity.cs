using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Tasks;
using CriThink.Client.Droid.SocialLogins;
using Firebase;
using Firebase.Auth;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace CriThink.Client.Droid.Views.Users
{
    public class BaseSocialLoginActivity<TViewModel> : MvxActivity<TViewModel>, IOnSuccessListener, IOnFailureListener where TViewModel : class, IMvxViewModel
    {
        private SocialLoginProvider _socialLoginProvider;
        private GoogleApiClient _googleApiClient;
        private GoogleSignInOptions _gso;
        private FirebaseAuth _firebaseAuth;
        private ICallbackManager _callbackManager;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (_socialLoginProvider == SocialLoginProvider.Google)
            {
                if (requestCode == 1)
                {
                    GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);

                    if (result.IsSuccess)
                    {
                        GoogleSignInAccount account = result.SignInAccount;
                        LoginWithFirebase(account);
                    }
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

            if (_gso == null || _googleApiClient == null)
                InitGoogleSignIn();

            if (_firebaseAuth.CurrentUser == null)
            {
                var intent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);
                StartActivityForResult(intent, 1);
            }
            else
            {
                _firebaseAuth.SignOut();
            }
        }

        private void InitGoogleSignIn()
        {
            _gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("1064440290860-5cdu2g1cnf4dbjj1afiikdiirkmb9ak4.apps.googleusercontent.com")
                .RequestEmail()
                .Build();

            _googleApiClient = new GoogleApiClient.Builder(this).AddApi(Auth.GOOGLE_SIGN_IN_API, _gso).Build();
            _googleApiClient.Connect();
            _firebaseAuth = GetFirebaseAuth();
        }

        private FirebaseAuth GetFirebaseAuth()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseAuth mAuth;

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("crithink-staging-1602540176980")
                    .SetApplicationId("crithink-staging-1602540176980")
                    .SetApiKey("AIzaSyBOnyKZgXJRj6l68JPP3p36KgCal0UwIks")
                    .SetDatabaseUrl("https://crithink-staging-1602540176980.firebaseio.com")
                    .SetStorageBucket("crithink-staging-1602540176980.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;
            }
            else
            {
                mAuth = FirebaseAuth.Instance;
            }
            return mAuth;
        }

        private void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            _firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this).AddOnFailureListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            //displayNameText.Text = "Display Name: " + firebaseAuth.CurrentUser.DisplayName;
            //emailText.Text = "Email: " + firebaseAuth.CurrentUser.Email;
            //photourlText.Text = "Photo URL: " + firebaseAuth.CurrentUser.PhotoUrl.Path;

            //Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
            //UpdateUI();
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            //Toast.MakeText(this, "Login Failed", ToastLength.Short).Show();
            //UpdateUI();
        }

        #endregion
    }
}