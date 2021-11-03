using System;
using Acr.UserDialogs;
using Android.App;
using Android.Graphics.Drawables;
using Android.Runtime;
#if (APPCENTER)
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
#endif
using MvvmCross.Platforms.Android.Views;

namespace CriThink.Client.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<Setup, Core.App>
    {
#if (APPCENTER)
        private const string AppCenterApiKey = "<APPCENTER_API_KEY>";
# endif

        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
#if (APPCENTER)
            AppCenter.Start(AppCenterApiKey, typeof(Analytics), typeof(Crashes));
#endif
            UserDialogs.Init(this);
            base.OnCreate();
        }

        public static void SetGradientStatusBar(Activity activity)
        {
            Drawable background = activity.Resources.GetDrawable(Resource.Drawable.gradient_background, null);
            activity.Window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            activity.Window.SetBackgroundDrawable(background);
        }
    }
}