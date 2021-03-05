using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;

namespace CriThink.Client.Droid.Singletons
{
    public static class GoogleSingleton
    {
        public static int ActivityResultCode => 1;

        private static GoogleSignInClient SignInClientInstance;

        private static GoogleSignInClient SignInClient => SignInClientInstance ??= InitGoogleSignIn();

        private static Context Context => Application.Context;

        public static async Task<string> RefreshTokenAsync()
        {
            var lastUser = GoogleSignIn.GetLastSignedInAccount(Context);
            if (lastUser is null)
                throw new InvalidOperationException("No Google account logged");

            var googleAccount = await SignInClient.SilentSignInAsync();
            return googleAccount.IdToken;
        }

        public static async Task LogoutAsync()
        {
            await SignInClient.SignOutAsync();
        }

        public static Intent GetLoginIntent()
        {
            return SignInClient.SignInIntent;
        }

        private static GoogleSignInClient InitGoogleSignIn()
        {
            if (Context is null)
                throw new InvalidOperationException("Android context is null");

            var token = Context.GetString(Resource.String.google_signin);

            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(token)
                .RequestEmail()
                .Build();

            return GoogleSignIn.GetClient(Context, gso);
        }
    }
}