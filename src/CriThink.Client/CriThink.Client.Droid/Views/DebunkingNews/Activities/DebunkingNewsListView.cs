using System.ComponentModel;
using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using Com.Facebook.Shimmer;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Droid.Controls;
using CriThink.Client.Droid.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;
using MvvmCross.WeakSubscription;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    [MvxActivityPresentation]
    [Activity]
    public class DebunkingNewsListView : MvxActivity<DebunkingNewsListViewModel>
    {
        private ShimmerFrameLayout _layoutShimmer;
        private AppCompatTextView _txtTitle;
        private MvxAppCompatSpinner _btnFilterCountry;
        private AppCompatButton _btnFilterLanguage;
        private ScrollView _scrollView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ViewModel.WeakSubscribe(() => ViewModel.IsLoading, UpdateShimmerAnimation);
            ViewModel.WeakSubscribe(() => ViewModel.FilterByCountry, UpdateCountryFilter);
            ViewModel.WeakSubscribe(() => ViewModel.FilterByLanguage, UpdateLanguageFilter);

            SetContentView(Resource.Layout.debunkingnewslist_view);
            MainApplication.SetGradientStatusBar(this);

            var loader = FindViewById<LoaderView>(Resource.Id.loader);
            var listDebunkingNews = FindViewById<MvxRecyclerView>(Resource.Id.recyclerDebunkingNews);
            _btnFilterCountry = FindViewById<MvxAppCompatSpinner>(Resource.Id.btnFilterCountry);
            _btnFilterLanguage = FindViewById<AppCompatButton>(Resource.Id.btnFilterLanguage);
            _scrollView = FindViewById<ScrollView>(Resource.Id.scrollview_shimmer);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            _txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var layoutManager = new LinearLayoutManager(this);
            listDebunkingNews.SetLayoutManager(layoutManager);
            listDebunkingNews.SetItemAnimator(null);

            var adapter = new DebunkingNewsAdapter((IMvxAndroidBindingContext) BindingContext);
            listDebunkingNews.Adapter = adapter;

            listDebunkingNews.AddOnScrollFetchItemsListener(
                layoutManager,
                () => ViewModel.FetchDebunkingNewsTask,
                () => ViewModel.FetchDebunkingNewsCommand);

            _layoutShimmer = FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_layout);
            _layoutShimmer.StartShimmerAnimation(ViewModel.IsLoading);

            var set = CreateBindingSet();

            set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Feed);
            set.Bind(listDebunkingNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);
            set.Bind(listDebunkingNews).For(v => v.Visibility).To(vm => vm.IsLoading)
                .WithConversion<MvxInvertedVisibilityValueConverter>();
            set.Bind(_txtTitle).ToLocalizationId("Title");
            set.Bind(_layoutShimmer).For(v => v.BindVisible()).To(vm => vm.IsLoading);
            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);

            set.Bind(_btnFilterCountry).For(v => v.ItemsSource).To(vm => vm.CountryFilters);
            set.Bind(_btnFilterCountry).For(v => v.SelectedItem).To(vm => vm.SelectedCountryFilter);
            set.Bind(_btnFilterCountry).For(v => v.HandleItemSelected).To(vm => vm.HandleCountryFilterSelectedCommand);

            set.Bind(_btnFilterLanguage).For(v => v.Text).ToLocalizationId("Language");
            //set.Bind(_btnFilterCountry).For(v => v.BindClick()).To(vm => vm.FIlterByCountryCommand);
            set.Bind(_btnFilterLanguage).For(v => v.BindClick()).To(vm => vm.FIlterByLanguageCommand);

            set.Apply();
        }

        private void ToggleFilter(MvxAppCompatSpinner button, bool filter)
        {
            RunOnUiThread(() =>
            {
                if (filter)
                {
                    button.SetBackgroundResource(Resource.Drawable.orange_bg_button_radius);
                    //button.SetTextColor(Color.White);
                }
                else
                {
                    button.SetBackgroundResource(Resource.Drawable.outline_bg_button_radius);
                    //button.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.borderUnselectedColor)));
                }
            });
        }

        private void UpdateLanguageFilter(object sender, PropertyChangedEventArgs e)
        {
            //ToggleFilter(_btnFilterLanguage, ViewModel.FilterByLanguage);
        }

        private void UpdateCountryFilter(object sender, PropertyChangedEventArgs e)
        {
            ToggleFilter(_btnFilterCountry, ViewModel.FilterByCountry);
        }

        private void UpdateShimmerAnimation(object sender, PropertyChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                _layoutShimmer?.StartShimmerAnimation(ViewModel.IsLoading);
                _scrollView.VisibleOrGone(ViewModel.IsLoading);
            });
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }
    }
}