using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Users
{
    [MvxFragmentPresentation(typeof(WelcomeLoginSignInViewModel), Resource.Id.pager)]
    [Register(ViewConstants.Namespace + ".users." + nameof(WelcomeLoginSignInView))]
    public class WelcomeLoginSignInView : MvxFragment<WelcomeLoginSignInViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.welcomeloginsignin_view, null);

            var btnLogin = view.FindViewById<AppCompatButton>(Resource.Id.btnLogin);
            var btnSignIn = view.FindViewById<AppCompatButton>(Resource.Id.btnSignIn);

            var set = CreateBindingSet();

            set.Bind(btnLogin).To(vm => vm.NavigateToLoginViewCommand);
            set.Bind(btnSignIn).To(vm => vm.NavigateToSignInViewCommand);

            set.Apply();

            return view;
        }
    }
}