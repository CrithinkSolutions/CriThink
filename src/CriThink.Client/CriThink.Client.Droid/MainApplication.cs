using System;
using Acr.UserDialogs;
using Android.App;
using Android.Runtime;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Platforms.Android.Views;

namespace CriThink.Client.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<Setup, Core.App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            AppCenter.Start("fac58211-3881-4d0b-bda8-8fe6e0ec9243", typeof(Analytics), typeof(Crashes));
            UserDialogs.Init(this);
            base.OnCreate();
        }
    }
}