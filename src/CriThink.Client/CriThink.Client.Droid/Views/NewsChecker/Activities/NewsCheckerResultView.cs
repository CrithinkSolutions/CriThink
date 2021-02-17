using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Core.Content;
using AndroidX.ViewPager.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using CriThink.Client.Droid.Views.DebunkingNews;
using CriThink.Client.Droid.Views.Users;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.ViewPager;
using MvvmCross.Plugin.Visibility;
using MvvmCross.ViewModels;
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
        private WelcomeViewFragmentAdapter _adapter;
        private ViewPager _pager;
        private IPageIndicator _indicator;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.newscheckerresult_view);

            BuildTitleText();
            BuildViewPager();

            var loader = FindViewById<LoaderView>(Resource.Id.layoutLoader);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var layoutResult = FindViewById<ConstraintLayout>(Resource.Id.layoutResult);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            SupportActionBar.SetCustomView(_txtTitle, new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent));

            var set = CreateBindingSet();

            set.Bind(_txtTitle).To(vm => vm.Title);
            set.Bind(loader).For(v => v.Visibility).To(vm => vm.IsLoading).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(layoutResult).For(v => v.Visibility).To(vm => vm.IsLoading).WithConversion<MvxInvertedVisibilityValueConverter>();

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

        private void BuildViewPager()
        {
            var fragments = new List<MvxViewPagerFragmentInfo>
            {
                new MvxViewPagerFragmentInfo(
                    nameof(NewsCheckerResultDetailView),
                    "",
                    typeof(NewsCheckerResultDetailView),
                    new MvxViewModelInstanceRequest(ViewModel.ResultDetailViewModel)),

                new MvxViewPagerFragmentInfo(
                    nameof(RelatedDebunkingNewsView),
                    "",
                    typeof(RelatedDebunkingNewsView),
                    new MvxViewModelInstanceRequest(ViewModel.FirstRelatedDebunkingNews)),

                new MvxViewPagerFragmentInfo(
                    nameof(RelatedDebunkingNewsView),
                    "",
                    typeof(RelatedDebunkingNewsView),
                    new MvxViewModelInstanceRequest(ViewModel.SecondRelatedDebunkingNews)),
            };

            _adapter = new WelcomeViewFragmentAdapter(this, SupportFragmentManager, fragments);
            _pager = FindViewById<ViewPager>(Resource.Id.pager);
            if (_pager != null)
                _pager.Adapter = _adapter;

            _indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
            if (_indicator is CirclePageIndicator circleIndicator)
            {
                circleIndicator.SetViewPager(_pager);
                circleIndicator.Snap = true;
                circleIndicator.PageColor = new Color(ContextCompat.GetColor(this, Resource.Color.welcomeBackground));
                circleIndicator.StrokeColor = new Color(ContextCompat.GetColor(this, Resource.Color.colorBlue));
            }
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