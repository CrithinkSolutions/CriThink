using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.SpotFakeNews;
using MvvmCross.Binding.BindingContext;
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

            var txtHDescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtHDescription);
            var txtEDescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtEDescription);
            var txtADescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtADescription);
            var txtDDescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtDDescription);

            var binder = this as IMvxBindingContextOwner;
            var set = binder.CreateBindingSet<IMvxBindingContextOwner, SpotFakeNewsHomeViewModel>();

            set.Bind(txtHDescription).ToLocalizationId("HDescription");
            set.Bind(txtEDescription).ToLocalizationId("EDescription");
            set.Bind(txtADescription).ToLocalizationId("ADescription");
            set.Bind(txtDDescription).ToLocalizationId("DDescription");

            set.Apply();

            return view;
        }
    }
}