using Android.App;
using Android.OS;
using CriThink.Client.Core.ViewModels.Users;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace CriThink.Client.Droid.Views.Users
{
    [MvxActivityPresentation]
    [Activity]
    public class SignUpView : MvxActivity<SignUpViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.signup_view);
        }
    }
}