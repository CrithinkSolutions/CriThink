using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Tasks;
using Android.OS;
using AndroidX.ViewPager.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Droid.SocialLogins;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using Firebase;
using Firebase.Auth;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.ViewPager;
using MvvmCross.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity(Label = "View for WelcomeViewModel")]
    public class WelcomeView : MvxActivity<WelcomeViewModel>, IOnSuccessListener, IOnFailureListener
    {
        private WelcomeViewFragmentAdapter _adapter;
        private ViewPager _pager;
        private IPageIndicator _indicator;
        private SocialLoginProvider _socialLoginProvider;
        private GoogleApiClient _googleApiClient;
        private GoogleSignInOptions _gso;
        private FirebaseAuth _firebaseAuth;
        private ICallbackManager _callbackManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.welcome_view);

            BuildViewPager();
        }

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

        private void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            _firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this).AddOnFailureListener(this);
        }

        private void BuildViewPager()
        {
            var fragments = new List<MvxViewPagerFragmentInfo>
            {
                new MvxViewPagerFragmentInfo(nameof(WelcomeFragment), "", typeof(WelcomeFragment), new MvxViewModelRequest()),
                new MvxViewPagerFragmentInfo(nameof(SignUpView), "", typeof(SignUpView), new MvxViewModelInstanceRequest(ViewModel.SignUpViewModel)),
            };

            _adapter = new WelcomeViewFragmentAdapter(this, SupportFragmentManager, fragments);
            _pager = FindViewById<ViewPager>(Resource.Id.pager);
            if (_pager != null)
                _pager.Adapter = _adapter;

            _indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
            if (_indicator is CirclePageIndicator circleIndicator)
            {
                circleIndicator.SetViewPager(_pager);
                circleIndicator.Snap = true;
            }
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
    }
}