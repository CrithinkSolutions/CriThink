using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Droid.Constants;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    [MvxFragmentPresentation(typeof(RelatedDebunkingNewsViewModel), "pager")]
    [Register(ViewConstants.Namespace + ".debunkingnews." + nameof(RelatedDebunkingNewsView))]
    public class RelatedDebunkingNewsView : MvxFragment<RelatedDebunkingNewsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.relateddebunkingnews_view, null);

            var txtTitle = view.FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtPublisher = view.FindViewById<AppCompatTextView>(Resource.Id.txtPublisher);
            var txtCaption = view.FindViewById<AppCompatTextView>(Resource.Id.txtCaption);
            var txtPublishingDate = view.FindViewById<AppCompatTextView>(Resource.Id.txtPublishingDate);
            var imgHeader = view.FindViewById<MvxCachedImageView>(Resource.Id.imgHeader);
            var btnOpenNews = view.FindViewById<AppCompatButton>(Resource.Id.btnOpenNews);

            var set = CreateBindingSet();

            set.Bind(txtTitle).To(vm => vm.Title);
            set.Bind(txtPublisher).To(vm => vm.Publisher);
            set.Bind(txtCaption).To(vm => vm.Caption);
            set.Bind(txtPublishingDate).To(vm => vm.PublishingDate);
            set.Bind(imgHeader).For(v => v.ImagePath).To(vm => vm.NewsImageLink);
            set.Bind(btnOpenNews).For(v => v.Text).ToLocalizationId("OpenNews");
            set.Bind(btnOpenNews).To(vm => vm.OpenDebunkingNewsCommand);

            set.Apply();

            return view;
        }
    }
}