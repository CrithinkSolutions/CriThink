using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Constants;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    [Register(ViewConstants.Namespace + ".newschecker." + nameof(NewsCheckerResultDetailView))]
    public class NewsCheckerResultDetailView : MvxFragment<NewsCheckerResultDetailViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.newscheckerresultdetail_view, null);

            var txtClassification = view.FindViewById<AppCompatTextView>(Resource.Id.txtClassification);
            var txtDescription = view.FindViewById<AppCompatTextView>(Resource.Id.txtDescription);
            var txtRelatedDNews = view.FindViewById<AppCompatTextView>(Resource.Id.txtRelatedDNews);

            var set = CreateBindingSet();

            set.Bind(txtRelatedDNews).ToLocalizationId("RelatedDNews");
            set.Bind(txtClassification).To(vm => vm.Classification);
            set.Bind(txtDescription).To(vm => vm.Description);

            set.Apply();

            return view;
        }
    }
}