using System;

using Android.App;
using Android.Content;
using Android.OS;
using CriThink.Client.Core.Auth;

namespace CriThink.Client.Droid.Views.Common
{
    [Activity(
        Label = "CustomUrlSchemeInterceptorActivity",
        NoHistory = true,
        LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "com.googleusercontent.apps.753941170579-f86rqbkmm7dg5p991qpavlnm6suh4rfp" },
        DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = new Uri(Intent.Data.ToString());

            AuthenticationState.Authenticator.OnPageLoading(uri);

            Finish();
        }
    }
}