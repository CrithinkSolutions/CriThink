using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Droid.Controls;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using ActionBar = AndroidX.AppCompat.App.ActionBar;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.DebunkingNewsDetailsView")]
    public class DebunkingNewsDetailsView : MvxActivity<DebunkingNewsDetailsViewModel>
    {
        private AppCompatTextView _txtTitle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.debunkingnewsdetails_view);

            BuildTitleText();

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var webView = FindViewById<BindableWebView>(Resource.Id.webView);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            SupportActionBar.SetCustomView(_txtTitle, new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent));

            var set = CreateBindingSet();

            set.Bind(_txtTitle).To(vm => vm.Title);
            set.Bind(webView).For(v => v.Uri).To(vm => vm.Uri);

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