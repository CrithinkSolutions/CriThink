using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.Content;
using AndroidX.Core.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Constants;
using CriThink.Client.Droid.Controls;
using CriThink.Client.Droid.Views.NewsChecker.Adapters;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.Card;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.WebviewNewsView")]
    public class WebviewNewsView : MvxActivity<WebviewNewsViewModel>
    {
        private Toolbar _toolbar;
        private BindableWebView _webViewNews;
        private CoordinatorLayout _mainView;
        private ConstraintLayout _bottomSheetHeader;
        private ConstraintLayout _bottomSheetLayout;
        private MaterialCardView _materialCardView;
        public WebviewNewsView()
        {
        }

        private BottomSheetBehavior _sheetBehavior;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.newschecker_webview_view);

            _toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            _webViewNews = FindViewById<BindableWebView>(Resource.Id.webViewNews);
            _materialCardView = FindViewById<MaterialCardView>(Resource.Id.cardWebView);
            _bottomSheetHeader = FindViewById<ConstraintLayout>(Resource.Id.bottomSheetHeader);
            var listQuestionNews = FindViewById<MvxRecyclerView>(Resource.Id.recyclerQuestions);
            _bottomSheetLayout = FindViewById<ConstraintLayout>(Resource.Id.bottomSheetLayout);
            _mainView = FindViewById<CoordinatorLayout>(Resource.Id.mainView);
            var layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            var adapter = new QuestionNewsAdapter((IMvxAndroidBindingContext) BindingContext);
            _sheetBehavior = BottomSheetBehavior.From(_bottomSheetLayout);
            _mainView.ViewTreeObserver.GlobalLayout += ViewTreeObserver_GlobalLayout;
            listQuestionNews.SetLayoutManager(layoutManager);
            listQuestionNews.Adapter = adapter;
            listQuestionNews.NestedScrollingEnabled = false;
            _webViewNews.UsePreventExternalWebViewClient();
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            Window.SetStatusBarColor(new Color(ContextCompat.GetColor(this, Resource.Color.accent)));
            using (var set = CreateBindingSet())
            {
                set.Bind(_webViewNews).For(v => v.Uri).To(vm => vm.Uri);
                set.Bind(_toolbar).For(v => v.Subtitle).To(vm => vm.Uri.Host);
                set.Bind(adapter).For(v => v.ItemsSource).To(vm => vm.Questions);
            }
            
        }

        private void ViewTreeObserver_GlobalLayout(object sender, EventArgs e)
        {
            _mainView.ViewTreeObserver.GlobalLayout -= ViewTreeObserver_GlobalLayout;

            var rectangle = new Rect();
            Window.DecorView.GetWindowVisibleDisplayFrame(rectangle);


            var statusBar = rectangle.Top;
            var height = _materialCardView.Height + _toolbar.Height + statusBar;
            
            _sheetBehavior.State = BottomSheetBehavior.StateCollapsed;
            _sheetBehavior.HalfExpandedRatio = (float)_materialCardView.Height / (float)height;
            _sheetBehavior.GestureInsetBottomIgnored = false;
            _sheetBehavior.FitToContents = false;
            _sheetBehavior.ExpandedOffset = (int) (_toolbar.Height + statusBar);
            _sheetBehavior.Hideable = false;
            _sheetBehavior.SkipCollapsed = false;
            _sheetBehavior.AddBottomSheetCallback(new BottomSheetToolbarToggleCallback());
        }

        public static float PxFromDp(Context context, float dp)
        {
            return dp * context.Resources.DisplayMetrics.Density;
        }

        public static float DpFromPx(Context context, float px)
        {
            return px / context.Resources.DisplayMetrics.Density;
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

    }

    public class BottomSheetToolbarToggleCallback : BottomSheetBehavior.BottomSheetCallback
    {
        public BottomSheetToolbarToggleCallback()
        {
        }
        public override void OnSlide(View bottomSheet, float slideOffset)
        {
        }
        public override void OnStateChanged(View bottomSheet, int newState)
        {
            switch (newState)
            {
                case BottomSheetBehavior.StateCollapsed:
                    ShowImageArrowUp(bottomSheet, true);
                    break;
                default:
                    ShowImageArrowUp(bottomSheet, false);
                    break;
            }
        }
        private void ShowImageArrowUp(View bottomSheet, bool isVisible)
        {
            var imageView = bottomSheet.FindViewById<ImageView>(Resource.Id.img_arrow_up);
            if (imageView != null)
            {
                imageView.Visibility = isVisible ? ViewStates.Visible : ViewStates.Invisible;
            }
        }
    }

}
