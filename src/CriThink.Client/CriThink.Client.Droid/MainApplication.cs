using System;
using Acr.UserDialogs;
using Android.App;
using Android.Runtime;
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
#if (!DEBUG)
            AppCenter.Start("fac58211-3881-4d0b-bda8-8fe6e0ec9243", typeof(Analytics), typeof(Crashes));
#endif
            UserDialogs.Init(this);
            base.OnCreate();
        }
    }
}