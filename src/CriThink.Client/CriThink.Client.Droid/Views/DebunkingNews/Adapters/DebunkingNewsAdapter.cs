using System;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Common.Endpoints.DTOs.Admin;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.DebunkingNews
{
    public class DebunkingNewsAdapter : MvxRecyclerAdapter
    {
        public DebunkingNewsAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        { }

        [Preserve(Conditional = true)]
        protected DebunkingNewsAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, Resource.Layout.cell_debunkingnews_feed, itemBindingContext);

            return new DebunkingNewsViewHolder(view, itemBindingContext);
        }
    }

    public class DebunkingNewsHorizontalAdapter : MvxRecyclerAdapter
    {
        public DebunkingNewsHorizontalAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        { }

        [Preserve(Conditional = true)]
        protected DebunkingNewsHorizontalAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, Resource.Layout.cell_debunkingnews_feed, itemBindingContext);
            view.LayoutParameters.Width = (int) (parent.Width * 0.7);

            return new DebunkingNewsViewHolder(view, itemBindingContext);
        }
    }

    public class DebunkingNewsViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _txtTitle;
        private readonly AppCompatTextView _txtPublisher;
        private readonly MvxCachedImageView _imgNews;

        public DebunkingNewsViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtTitle = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            _txtPublisher = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtPublisher);
            _imgNews = itemView.FindViewById<MvxCachedImageView>(Resource.Id.imgNews);

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DebunkingNewsViewHolder, DebunkingNewsGetResponse>();

                set.Bind(_txtTitle).To(x => x.Title);
                set.Bind(_txtPublisher).To(x => x.Publisher);
                set.Bind(_imgNews).For(v => v.ImagePath).To(vm => vm.NewsImageLink);

                set.Apply();
            });
        }
    }
}