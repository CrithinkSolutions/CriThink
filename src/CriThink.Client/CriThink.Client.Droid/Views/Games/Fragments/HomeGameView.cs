using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Games;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Games
{
    [MvxFragmentPresentation(typeof(HomeViewModel), "tabs_container_frame")]
    [Register(nameof(HomeGameView))]
    public class HomeGameView : MvxFragment<HomeGameViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.homegame_view, null);

            var txtWip = view.FindViewById<AppCompatTextView>(Resource.Id.txtWip);

            var set = CreateBindingSet();

            set.Bind(txtWip).ToLocalizationId("Wip");

            set.Apply();

            return view;
        }
    }
}