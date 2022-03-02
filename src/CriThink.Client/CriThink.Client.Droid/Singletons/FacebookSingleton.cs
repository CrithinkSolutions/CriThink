using System;
using Android.App;
using Android.Content;
using CriThink.Client.Droid.SocialLogins;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace CriThink.Client.Droid.Singletons
{
    public static class FacebookSingleton
    {
        private static ICallbackManager CallbackManager;

        private static ICallbackManager LoginCallbackManager => CallbackManager ??= Initialize();

        public static void Login(Activity activity, FacebookCallback<LoginResult> callbacks)
        {
            LoginManager.Instance.LogInWithReadPermissions(activity, new[]
            {
                "email",
                "public_profile",
                "user_gender",
                "user_birthday",
                "user_location"
            });

            LoginManager.Instance.RegisterCallback(LoginCallbackManager, callbacks);
        }

        public static string RefreshFacebookToken()
        {
            AccessToken.RefreshCurrentAccessTokenAsync();
            if (AccessToken.IsCurrentAccessTokenActive)
                return AccessToken.CurrentAccessToken.Token;

            throw new InvalidOperationException("No Facebook account logged");
        }

        public static bool OnActivityResult(int requestCode, Result result, Intent data)
        {
            return LoginCallbackManager.OnActivityResult(requestCode, (int) result, data);
        }

        public static void Logout()
        {
            if (AccessToken.CurrentAccessToken != null)
            {
                LoginManager.Instance.LogOut();
            }
        }

        private static ICallbackManager Initialize() =>
            CallbackManagerFactory.Create();
    }
}