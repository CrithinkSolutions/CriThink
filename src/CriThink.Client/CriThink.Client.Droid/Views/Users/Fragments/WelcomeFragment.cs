using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.Users;
using CriThink.Client.Droid.Constants;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
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

        public string ImageName { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.welcomeimage_view, null);

            var image = view.FindViewById<AppCompatImageView>(Resource.Id.img);

            ImageService.Instance
                .LoadCompiledResource(ImageName)
                .WithCustomDataResolver(new SvgDataResolver())
                .Into(image);

            return view;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null && savedInstanceState.ContainsKey(KeyContent))
                ImageName = savedInstanceState.GetString(KeyContent);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutString(KeyContent, ImageName);
        }
    }
}