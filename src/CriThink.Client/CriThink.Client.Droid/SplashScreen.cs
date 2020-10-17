﻿using Android.App;
using Android.Content.PM;
using MvvmCross.Platforms.Android.Views;

namespace CriThink.Client.Droid
{
    [Activity(
        Label = "CriThink.Client.Droid"
        , MainLauncher = true
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        { }
    }
}