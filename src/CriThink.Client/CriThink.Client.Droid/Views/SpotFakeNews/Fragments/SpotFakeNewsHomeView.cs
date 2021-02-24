using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
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
        private static bool IsInitialized;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.spotfakenewshome_view, null);

            var txtHDescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtHDescription);
            var txtEDescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtEDescription);
            var txtADescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtADescription);
            var txtDDescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtDDescription);
            var layoutH = view.FindViewById<ConstraintLayout>(Resource.Id.layoutH);
            var layoutE = view.FindViewById<ConstraintLayout>(Resource.Id.layoutE);
            var layoutA = view.FindViewById<ConstraintLayout>(Resource.Id.layoutA);
            var layoutD = view.FindViewById<ConstraintLayout>(Resource.Id.layoutD);

            var set = CreateBindingSet();

            set.Bind(txtHDescription).ToLocalizationId("HDescription");
            set.Bind(txtEDescription).ToLocalizationId("EDescription");
            set.Bind(txtADescription).ToLocalizationId("ADescription");
            set.Bind(txtDDescription).ToLocalizationId("DDescription");

            set.Apply();

            if (!IsInitialized)
            {
                var bounceAnimation = AnimationUtils.LoadAnimation(Activity, Resource.Animation.bounce_animation);
                layoutH.StartAnimation(bounceAnimation);
                layoutE.StartAnimation(bounceAnimation);
                layoutA.StartAnimation(bounceAnimation);
                layoutD.StartAnimation(bounceAnimation);
            }

            IsInitialized = true;

            return view;
        }
    }
}