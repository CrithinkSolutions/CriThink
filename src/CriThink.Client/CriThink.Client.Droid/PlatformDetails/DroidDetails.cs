using Android.Content;
using CriThink.Client.Core.Platform;

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

        public void OpenSkypeProfile(string profileName)
        {
            var skypeIntent = GetIntentOfTheGivenPackage("com.skype.raider");
            var skypeUri = "https://web.skype.com/";
            if (skypeIntent != null)
            {
                skypeUri = $"skype:{profileName}";
            }
            LaunchIntent(skypeUri);
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