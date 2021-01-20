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
            UserDialogs.Init(this);
            base.OnCreate();
        }
    }
}