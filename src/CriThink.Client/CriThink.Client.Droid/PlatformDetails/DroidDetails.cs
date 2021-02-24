using System;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using CriThink.Client.Core.Platform;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace CriThink.Client.Droid.PlatformDetails
{
    /// <summary>
    /// <see cref="IPlatformDetails"/> implementation for Android
    /// </summary>
    public class DroidDetails : IPlatformDetails
    {
        public void OpenFacebook(string pageId = null)
        {
            // the intent returned by GetLaunchIntentForPackage does not open correctly Facebook app for some reason
            var fbIntent = GetIntentOfTheGivenPackage("com.facebook.katana");
            var fbUri = $"https://www.facebook.com/{pageId}";
            if (fbIntent != null)
            {
                fbUri = $"fb://facewebmodal/f?href={fbUri}";
            }

            LaunchIntent(fbUri);
        }

        public void OpenInstagramProfile(string profileId = null)
        {
            var igUri = $"https://www.instagram.com/_u/{profileId}";
            LaunchIntent(igUri);
        }

        public void OpenTwitterProfile(string profileId = null)
        {
            var twitterIntent = GetIntentOfTheGivenPackage("com.twitter.android");
            var twitterUri = "https://twitter.com/crithinktech";
            if (twitterIntent != null)
            {
                twitterUri = $"twitter://user?user_id={profileId}";
            }

            LaunchIntent(twitterUri);
        }

        public void OpenLinkedInProfile(string profileId = null)
        {
            var linkedInUri = $"https://www.linkedin.com/{profileId}";
            LaunchIntent(linkedInUri);
        }


        public void LogoutSocialLogin()
        {
            var signInClient = InitGoogleSignIn();
            signInClient.SignOut();

            if (AccessToken.CurrentAccessToken != null)
            {
                LoginManager.Instance.LogOut();
            }
        }

        private static GoogleSignInClient InitGoogleSignIn()
        {
            var context = Android.App.Application.Context;
            if (context is null)
                throw new InvalidOperationException("Android context is null");

            var token = context.GetString(Resource.String.google_signin);

            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(token)
                .RequestEmail()
                .Build();

            return GoogleSignIn.GetClient(context, gso);
        }

        private static Intent GetIntentOfTheGivenPackage(string package) =>
            Android.App.Application.Context.PackageManager?.GetLaunchIntentForPackage(package);

        private static void LaunchIntent(string uri)
        {
            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
            intent.SetFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}