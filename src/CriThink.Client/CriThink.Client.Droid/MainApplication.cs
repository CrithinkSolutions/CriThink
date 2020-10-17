using System;

using Android.App;
using Android.Runtime;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;

namespace CriThink.Client.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<MvxAndroidSetup<Core.App>, Core.App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}