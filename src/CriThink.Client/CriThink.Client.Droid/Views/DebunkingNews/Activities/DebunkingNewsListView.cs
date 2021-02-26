using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Droid.Controls;
using CriThink.Client.Droid.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;
using ActionBar = AndroidX.AppCompat.App.ActionBar;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    [MvxActivityPresentation]
    [Activity]
    public class DebunkingNewsListView : MvxActivity<DebunkingNewsListViewModel>
    {
        private AppCompatTextView _txtTitle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.debunkingnewslist_view);

            var loader = FindViewById<LoaderView>(Resource.Id.loader);
            var listDebunkingNews = FindViewById<MvxRecyclerView>(Resource.Id.recyclerDebunkingNews);

            BuildTitleText();

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            SupportActionBar.SetCustomView(_txtTitle, new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent));

            var layoutManager = new LinearLayoutManager(this);
            listDebunkingNews.SetLayoutManager(layoutManager);
            listDebunkingNews.SetItemAnimator(null);

            var adapter = new DebunkingNewsAdapter((IMvxAndroidBindingContext) BindingContext);
            listDebunkingNews.Adapter = adapter;

            listDebunkingNews.AddOnScrollFetchItemsListener(
                layoutManager,
                () => ViewModel.FetchDebunkingNewsTask,
                () => ViewModel.FetchDebunkingNewsCommand);

            var set = CreateBindingSet();

            set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Feed);
            set.Bind(listDebunkingNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);
            set.Bind(listDebunkingNews).For(v => v.Visibility).To(vm => vm.IsLoading)
                .WithConversion<MvxInvertedVisibilityValueConverter>();
            set.Bind(_txtTitle).ToLocalizationId("Title");

            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

        private void BuildTitleText()
        {
            _txtTitle = new AppCompatTextView(this);
            _txtTitle.SetTextSize(ComplexUnitType.Sp, 14);
            _txtTitle.Gravity = GravityFlags.Center;
            _txtTitle.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }
    }
}