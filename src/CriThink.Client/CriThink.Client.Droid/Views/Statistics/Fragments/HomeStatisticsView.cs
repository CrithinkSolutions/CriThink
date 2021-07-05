using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.Statistics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Plugin.Visibility;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.Statistics
{
    [MvxFragmentPresentation(typeof(HomeViewModel), "tabs_container_frame")]
    [Register(nameof(HomeStatisticsView))]
    public class HomeStatisticsView : MvxFragment<HomeStatisticsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.homestatistics_view, null);

            var txtMottoOne = view.FindViewById<AppCompatTextView>(Resource.Id.txtMottoOne);
            var txtMottoTwo = view.FindViewById<AppCompatTextView>(Resource.Id.txtMottoTwo);
            var txtActiveUsersHeader = view.FindViewById<AppCompatTextView>(Resource.Id.txtActiveUsersHeader);
            var txtActiveUsers = view.FindViewById<AppCompatTextView>(Resource.Id.txtActiveUsers);
            var txtTotalSearchesHeader = view.FindViewById<AppCompatTextView>(Resource.Id.txtTotalSearchesHeader);
            var txtTotalSearches = view.FindViewById<AppCompatTextView>(Resource.Id.txtTotalSearches);
            var txtUserSearchesHeader = view.FindViewById<AppCompatTextView>(Resource.Id.txtUserSearchesHeader);
            var txtUserSearches = view.FindViewById<AppCompatTextView>(Resource.Id.txtUserSearches);
            var viewContent = view.FindViewById<ConstraintLayout>(Resource.Id.viewContent);
            var viewError = view.FindViewById<ConstraintLayout>(Resource.Id.viewError);
            var txtError = view.FindViewById<AppCompatTextView>(Resource.Id.txtError);

            var set = CreateBindingSet();

            set.Bind(txtActiveUsers).To(vm => vm.PlatformUsers);
            set.Bind(txtTotalSearches).To(vm => vm.PlatformSearches);
            set.Bind(txtUserSearches).To(vm => vm.UserSearches);

            set.Bind(viewContent).For(v => v.Visibility).To(vm => vm.HasFailed).WithConversion<MvxInvertedVisibilityValueConverter>();
            set.Bind(viewError).For(v => v.BindVisible()).To(vm => vm.HasFailed);

            set.Bind(txtMottoOne).ToLocalizationId("MottoOne");
            set.Bind(txtMottoTwo).ToLocalizationId("MottoTwo");
            set.Bind(txtActiveUsersHeader).ToLocalizationId("ActiveUsers");
            set.Bind(txtTotalSearchesHeader).ToLocalizationId("TotalSearches");
            set.Bind(txtUserSearchesHeader).ToLocalizationId("UserSearches");
            set.Bind(txtError).ToLocalizationId("Error");

            set.Apply();

            return view;
        }
    }
}