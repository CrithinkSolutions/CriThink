using Android.OS;
using Android.Runtime;
using Android.Views;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Games;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Games
{
    [MvxFragmentPresentation(typeof(HomeViewModel), null)]
    [Register(nameof(HomeGameView))]
    public class HomeGameView : MvxFragment<HomeGameViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.homegame_view, null);

            return view;
        }
    }
}