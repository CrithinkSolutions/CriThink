using Android.OS;
using Android.Runtime;
using Android.Views;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace CriThink.Client.Droid.Views.Users
{
    [MvxFragmentPresentation(typeof(WelcomeViewModel), Resource.Id.pager)]
    [Register(ViewConstants.Namespace + ".users." + nameof(SignUpEmailView))]
    public class SignUpEmailView : MvxFragment<SignUpEmailViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.signupemail_view, null);

            return view;
        }
    }
}