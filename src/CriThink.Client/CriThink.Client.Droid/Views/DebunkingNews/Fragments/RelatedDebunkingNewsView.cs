using Android.OS;
using Android.Runtime;
using Android.Views;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Droid.Constants;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    [Register(ViewConstants.Namespace + ".debunkingnews." + nameof(RelatedDebunkingNewsView))]
    public class RelatedDebunkingNewsView : MvxFragment<RelatedDebunkingNewsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.relateddebunkingnews_view, null);

            var set = CreateBindingSet();

            set.Apply();

            return view;
        }
    }
}