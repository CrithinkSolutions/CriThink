using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using AndroidX.ConstraintLayout.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Controls;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Visibility;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [MvxActivityPresentation]
    [Activity]
    public class NewsCheckerResultView : MvxActivity<NewsCheckerResultViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.newscheckerresult_view);

            var txtClassification = FindViewById<AppCompatTextView>(Resource.Id.txtClassification);
            var txtDescription = FindViewById<AppCompatTextView>(Resource.Id.txtDescription);
            var loader = FindViewById<LoaderView>(Resource.Id.layoutLoader);
            var txtToolbarTitle = FindViewById<AppCompatTextView>(Resource.Id.txtToolbarTitle);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var layoutResult = FindViewById<ConstraintLayout>(Resource.Id.layoutResult);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var set = CreateBindingSet();

            set.Bind(txtToolbarTitle).To(vm => vm.Title);
            set.Bind(txtClassification).To(vm => vm.Classification);
            set.Bind(txtDescription).To(vm => vm.Description);
            set.Bind(loader).For(v => v.Visibility).To(vm => vm.IsLoading).WithConversion<MvxVisibilityValueConverter>();
            set.Bind(layoutResult).For(v => v.Visibility).To(vm => vm.IsLoading).WithConversion<MvxInvertedVisibilityValueConverter>();

            set.Apply();
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return false;
        }
    }
}