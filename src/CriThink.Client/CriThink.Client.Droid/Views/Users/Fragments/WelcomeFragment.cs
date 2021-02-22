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
    [MvxFragmentPresentation(typeof(WelcomeFragment), "pager")]
    [Register(ViewConstants.Namespace + ".users." + nameof(WelcomeFragment))]
    public class WelcomeFragment : MvxFragment<WelcomeImageViewModel>
    {
        private const string KeyContent = "WelcomeFragment:Content";

        public int ImageId { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.welcomeimage_view, null);

            var image = view.FindViewById<AppCompatImageView>(Resource.Id.img);

            image.SetImageResource(ImageId);

            return view;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null && savedInstanceState.ContainsKey(KeyContent))
                ImageId = savedInstanceState.GetInt(KeyContent);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt(KeyContent, ImageId);
        }
    }
}