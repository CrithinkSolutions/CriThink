using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels;
using CriThink.Client.Core.ViewModels.DebunkingNews;
using CriThink.Client.Droid.Extensions;
using CriThink.Client.Droid.Views.DebunkingNews.Adapters;
using MvvmCross.DroidX;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    [MvxFragmentPresentation(typeof(HomeViewModel), null)]
    [Register(nameof(DebunkingNewsView))]
    public class DebunkingNewsView : MvxFragment<DebunkingNewsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.debunkingnews_view, null);

            var listDebunkingNews = view.FindViewById<MvxRecyclerView>(Resource.Id.list_debunkingNews);
            var refresher = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);

            if (listDebunkingNews != null)
            {
                listDebunkingNews.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                listDebunkingNews.SetLayoutManager(layoutManager);

                listDebunkingNews.Adapter = new DebunkingNewsAdapter(BindingContext as IMvxAndroidBindingContext);

                listDebunkingNews.AddOnScrollFetchItemsListener(
                    layoutManager,
                    () => ViewModel.FetchDebunkingNewsTask,
                    () => ViewModel.FetchDebunkingNewsCommand);
            }

            var set = CreateBindingSet();

            set.Bind(listDebunkingNews).For(v => v.ItemsSource).To(vm => vm.Feed);
            set.Bind(listDebunkingNews).For(v => v.ItemClick).To(vm => vm.DebunkingNewsSelectedCommand);

            set.Bind(refresher).For(v => v.Refreshing).To(vm => vm.IsRefreshing);
            set.Bind(refresher).For(v => v.RefreshCommand).To(vm => vm.RefreshDebunkingNewsCommand);

            set.Apply();

            return view;
        }
    }
}