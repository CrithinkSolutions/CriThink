using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using MvvmCross.Binding.BindingContext;
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

            var txtMotto = view.FindViewById<AppCompatTextView>(Resource.Id.txtMotto);
            var btnLogin = view.FindViewById<AppCompatButton>(Resource.Id.btnLogin);
            var btnSignIn = view.FindViewById<AppCompatButton>(Resource.Id.btnSignIn);
            var btnFacebook = view.FindViewById<AppCompatImageButton>(Resource.Id.imgFacebook);
            var btnInstagram = view.FindViewById<AppCompatImageButton>(Resource.Id.imgInstagram);
            var btnTwitter = view.FindViewById<AppCompatImageButton>(Resource.Id.imgTwitter);
            var btnLinkedin = view.FindViewById<AppCompatImageButton>(Resource.Id.imgLinkedIn);

            var set = CreateBindingSet();

            set.Bind(btnFacebook).For("Click").To(vm => vm.OpenFacebookPageCommand);
            set.Bind(btnInstagram).For("Click").To(vm => vm.OpenInstagramProfileCommand);
            set.Bind(btnTwitter).For("Click").To(vm => vm.OpenTwitterProfileCommand);
            set.Bind(btnLinkedin).For("Click").To(vm => vm.OpenLinkedInProfileCommand);

            set.Bind(txtMotto).ToLocalizationId("Motto");

            set.Bind(btnLogin).To(vm => vm.NavigateToLoginViewCommand);
            set.Bind(btnLogin).For(v => v.Text).ToLocalizationId("Login");
            set.Bind(btnSignIn).To(vm => vm.NavigateToSignInViewCommand);
            set.Bind(btnSignIn).For(v => v.Text).ToLocalizationId("SignIn");

            set.Apply();

            return view;
        }
    }
}