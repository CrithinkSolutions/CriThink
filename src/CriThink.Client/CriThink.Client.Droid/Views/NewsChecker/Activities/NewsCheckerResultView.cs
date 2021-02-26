using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using CriThink.Client.Droid.Views.DebunkingNews;
using FFImageLoading.Cross;
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
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity]
    public class NewsCheckerResultView : MvxActivity<NewsCheckerResultViewModel>
    {
        private AppCompatTextView _txtTitle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.newscheckerresult_view);

            BuildTitleText();

            var loader = FindViewById<LoaderView>(Resource.Id.loader);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var imgHeader = FindViewById<MvxSvgCachedImageView>(Resource.Id.imgHeader);
            var imgArrow = FindViewById<MvxCachedImageView>(Resource.Id.imgArrow);
            var txtClassification = FindViewById<AppCompatTextView>(Resource.Id.txtClassification);
            var txtDescription = FindViewById<AppCompatTextView>(Resource.Id.txtDescription);
            var txtRelatedDNews = FindViewById<AppCompatTextView>(Resource.Id.txtRelatedDNews);
            var recyclerRelatedDNews = FindViewById<MvxRecyclerView>(Resource.Id.recyclerRelatedDNews);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            SupportActionBar.SetCustomView(_txtTitle, new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent));

            var layoutManager = new LinearLayoutManager(this);
            recyclerRelatedDNews.SetLayoutManager(layoutManager);
            recyclerRelatedDNews.SetItemAnimator(null);

            var adapter = new RelatedDebunkingNewsAdapter((IMvxAndroidBindingContext) BindingContext);
            recyclerRelatedDNews.Adapter = adapter;

            var set = CreateBindingSet();

            set.Bind(_txtTitle).To(vm => vm.Title);
            set.Bind(imgHeader).For(v => v.ImagePath).To(vm => vm.ResultImage);
            set.Bind(txtRelatedDNews).ToLocalizationId("RelatedDNews");
            set.Bind(txtClassification).To(vm => vm.Classification);
            set.Bind(txtDescription).To(vm => vm.Description);
            set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Feed);

            set.Bind(recyclerRelatedDNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);

            set.Bind(loader).For(v => v.BindVisible()).To(vm => vm.IsLoading);
            set.Bind(txtRelatedDNews).For(v => v.Visibility).To(vm => vm.HasRelatedDebunkingNews).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(imgArrow).For(v => v.Visibility).To(vm => vm.HasRelatedDebunkingNews).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(recyclerRelatedDNews).For(v => v.Visibility).To(vm => vm.HasRelatedDebunkingNews).WithConversion<MvxVisibilityValueConverter>();

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