using System.ComponentModel;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Com.Facebook.Shimmer;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;
using MvvmCross.WeakSubscription;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity]
    public class SearchTextResultView : MvxActivity<SearchTextResultViewModel>
    {
        private ShimmerFrameLayout _layoutShimmer;
        private AppCompatTextView _txtTitle;
        private AppCompatButton _btnDebunkingNews;
        private AppCompatButton _btnCommunity;
        private ScrollView _scrollView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ViewModel.WeakSubscribe(() => ViewModel.IsLoading, UpdateShimmerAnimation);
            ViewModel.WeakSubscribe(() => ViewModel.FilterByDebunkingNews, UpdateDebunkingNewsFilter);
            ViewModel.WeakSubscribe(() => ViewModel.FilterByCommunity, UpdateCommunityFilter);

            SetContentView(Resource.Layout.searchtextresult_view);
            MainApplication.SetGradientStatusBar(this);

            var listNews = FindViewById<MvxRecyclerView>(Resource.Id.recyclerNews);
            _btnCommunity = FindViewById<AppCompatButton>(Resource.Id.btnCommunity);
            _btnDebunkingNews = FindViewById<AppCompatButton>(Resource.Id.btnDebunkingNews);
            _scrollView = FindViewById<ScrollView>(Resource.Id.scrollview_shimmer);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            _txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);

            var layoutManager = new LinearLayoutManager(this);
            listNews.SetLayoutManager(layoutManager);
            listNews.SetItemAnimator(null);

            var adapter = new NewsSourceSearchAdapter((IMvxAndroidBindingContext) BindingContext);
            listNews.Adapter = adapter;

            listNews.ItemTemplateSelector = new NewsSourceSearchSelector();

            _layoutShimmer = FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_layout);
            _layoutShimmer.StartShimmerAnimation(ViewModel.IsLoading);

            var set = CreateBindingSet();

            set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Feed);
            set.Bind(listNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);
            set.Bind(listNews).For(v => v.Visibility).To(vm => vm.IsLoading)
                .WithConversion<MvxInvertedVisibilityValueConverter>();
            set.Bind(_txtTitle).ToLocalizationId("Title");
            set.Bind(_layoutShimmer).For(v => v.BindVisible()).To(vm => vm.IsLoading);
            set.Bind(_btnDebunkingNews).For(v => v.Text).ToLocalizationId("DebunkingNews");
            set.Bind(_btnCommunity).For(v => v.Text).ToLocalizationId("Community");
            set.Bind(_btnDebunkingNews).For(v => v.BindClick()).To(vm => vm.FilterByDebunkingNewsCommand);
            set.Bind(_btnCommunity).For(v => v.BindClick()).To(vm => vm.FilterByCommunityCommand);

            set.Apply();
        }

        private void ToggleFilter(AppCompatButton button, bool filter)
        {
            RunOnUiThread(() =>
            {
                if (filter)
                {
                    button.SetBackgroundResource(Resource.Drawable.orange_bg_button_radius);
                    button.SetTextColor(Color.White);
                }
                else
                {
                    button.SetBackgroundResource(Resource.Drawable.outline_bg_button_radius);
                    button.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.borderUnselectedColor)));
                }
            });
        }

        private void UpdateDebunkingNewsFilter(object sender, PropertyChangedEventArgs e)
        {
            ToggleFilter(_btnDebunkingNews, ViewModel.FilterByDebunkingNews);
        }

        private void UpdateCommunityFilter(object sender, PropertyChangedEventArgs e)
        {
            ToggleFilter(_btnCommunity, ViewModel.FilterByCommunity);
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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Task.Run(() => ViewModel.NavigateToHomeAsync());
            return true;
        }

        public override void OnBackPressed()
        {
            Task.Run(() => ViewModel.NavigateToHomeAsync());
        }
    }
}