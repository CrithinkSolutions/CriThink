
using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.DebunkingNewsDetailsView")]
    public class DebunkingNewsDetailsView : MvxActivity<DebunkingNewsDetailsViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.debunkingnewsdetails_view);

            var txtTitle = FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtCaption = FindViewById<AppCompatTextView>(Resource.Id.txtCaption);

            var set = CreateBindingSet();

            set.Bind(txtTitle).To(vm => vm.DebunkingNews.Title);
            set.Bind(txtCaption).To(vm => vm.DebunkingNews.Caption);

            set.Apply();
        }
    }
}