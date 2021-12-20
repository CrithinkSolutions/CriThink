using System;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace CriThink.Client.Droid.Views.NewsChecker
{
    public class NewsSourceSearchAdapter : MvxRecyclerAdapter
    {
        public NewsSourceSearchAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        { }

        [Preserve(Conditional = true)]
        protected NewsSourceSearchAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (Resource.Layout.cell_search_community == viewType)
            {
                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
                var view = InflateViewForHolder(parent, Resource.Layout.cell_search_community, itemBindingContext);

                return new NewsSourceSearchCommunityViewHolder(view, itemBindingContext);
            }
            else if (Resource.Layout.cell_search_debunkingnews == viewType)
            {
                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
                var view = InflateViewForHolder(parent, Resource.Layout.cell_search_debunkingnews, itemBindingContext);

                return new NewsSourceSearchDebunkingNewsViewHolder(view, itemBindingContext);
            }
            else
            {
                return base.OnCreateViewHolder(parent, viewType);
            }
        }
    }

    public class NewsSourceSearchCommunityViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _txtTitle;
        private readonly AppCompatTextView _txtRate;
        private readonly MvxCachedImageView _imgFavIcon;

        public NewsSourceSearchCommunityViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtTitle = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            _txtRate = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtRate);
            _imgFavIcon = itemView.FindViewById<MvxCachedImageView>(Resource.Id.imgFavIcon);

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<NewsSourceSearchCommunityViewHolder, NewsSourceSearchByTextResponse>();

                set.Bind(_txtTitle).To(vm => vm.Title);
                set.Bind(_txtRate).SourceDescribed("'Community Rating: ' + Rate");
                //set.Bind(_txtRate).To(vm => vm.Rate).WithConversion("StringFormat", "Community Rating: {0}");
                set.Bind(_imgFavIcon).For(v => v.ImagePath).To(vm => vm.FavIcon);

                set.Apply();
            });
        }
    }

    public class NewsSourceSearchDebunkingNewsViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _txtTitle;
        private readonly AppCompatTextView _txtPublisher;
        private readonly MvxCachedImageView _imgNews;

        public NewsSourceSearchDebunkingNewsViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtTitle = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            _txtPublisher = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtPublisher);
            _imgNews = itemView.FindViewById<MvxCachedImageView>(Resource.Id.imgNews);

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<NewsSourceSearchDebunkingNewsViewHolder, NewsSourceRelatedDebunkingNewsResponse>();

                set.Bind(_txtTitle).To(vm => vm.Title);
                set.Bind(_txtPublisher).To(vm => vm.Publisher);
                set.Bind(_imgNews).For(v => v.ImagePath).To(vm => vm.NewsImageLink);

                set.Apply();
            });
        }
    }
}