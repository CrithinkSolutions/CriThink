using Android.OS;
using Android.Runtime;
using Android.Views;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.SpotFakeNews;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.SpotFakeNews
{
    [MvxFragmentPresentation(typeof(HomeViewModel), "tabs_container_frame")]
    [Register(nameof(SpotFakeNewsHomeView))]
    public class SpotFakeNewsHomeView : MvxFragment<SpotFakeNewsHomeViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.spotfakenewshome_view, null);

            return view;
        }
    }
}