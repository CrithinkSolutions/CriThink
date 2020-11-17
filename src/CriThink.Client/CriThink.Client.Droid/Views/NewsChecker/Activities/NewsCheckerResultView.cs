using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity(Label = "CriThink.NewsCheckerResultView")]
    public class NewsCheckerResultView : MvxActivity<NewsCheckerResultViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.newscheckerresult_view);

            var txtClassification = FindViewById<AppCompatTextView>(Resource.Id.txtClassification);
            var txtDescription = FindViewById<AppCompatTextView>(Resource.Id.txtDescription);

            var set = CreateBindingSet();

            set.Bind(txtClassification).To(vm => vm.Classification);
            set.Bind(txtDescription).To(vm => vm.Description);

            set.Apply();
        }
    }
}