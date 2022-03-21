using Android.App;
using Android.Content;
using Android.Content.PM;
using CriThink.Common.Endpoints;

namespace CriThink.Client.Droid.Views.Common.Activities
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = EndpointConstants.SchemaName)]
    public class WebAuthenticationCallbackActivity : Xamarin.Essentials.WebAuthenticatorCallbackActivity
    {
    }
}