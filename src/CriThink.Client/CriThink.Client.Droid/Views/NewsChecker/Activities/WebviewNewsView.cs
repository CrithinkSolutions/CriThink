using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Webkit;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using MvvmCross.DroidX.Material;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.WebviewNewsView")]
    public class WebviewNewsView : MvxActivity<WebviewNewsViewModel>
    {

        public WebviewNewsView()
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.newschecker_webview_view);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var webViewNews = FindViewById<BindableWebView>(Resource.Id.webViewNews);
            webViewNews.UsePreventExternalWebViewClient();
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayOptions((int) ActionBarDisplayOptions.ShowCustom, (int) ActionBarDisplayOptions.ShowCustom);
            Window.SetStatusBarColor(new Color(ContextCompat.GetColor(this, Resource.Color.accent)));

            using (var set = CreateBindingSet())
            {
                set.Bind(webViewNews).For(v => v.Uri).To(vm => vm.Uri);
                set.Bind(toolbar).For(v => v.Subtitle).To(vm => vm.Uri.Host);
            }
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }

    }
}
