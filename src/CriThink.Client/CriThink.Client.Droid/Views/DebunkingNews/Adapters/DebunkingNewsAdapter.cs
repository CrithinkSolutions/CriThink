using System;
using Android.Runtime;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Common.Endpoints.DTOs.Admin;
using FFImageLoading;
using FFImageLoading.Cross;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace CriThink.Client.Droid.Views.DebunkingNews.Adapters
{
    public class DebunkingNewsAdapter : MvxRecyclerAdapter
    {
        public DebunkingNewsAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        { }

        [Android.Runtime.Preserve(Conditional = true)]
        protected DebunkingNewsAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var txtTitle = holder.ItemView.FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            var txtPublisher = holder.ItemView.FindViewById<AppCompatTextView>(Resource.Id.txtPublisher);
            var imgNews = holder.ItemView.FindViewById<MvxCachedImageView>(Resource.Id.imgNews);

            if (GetItem(position) is DebunkingNewsGetResponse debunkingNews)
            {
                txtTitle.Text = debunkingNews.Title;
                txtPublisher.Text = debunkingNews.Publisher;
                ImageService.Instance.LoadUrl(debunkingNews.NewsImageLink)
                    .Into(imgNews);
            }

            base.OnBindViewHolder(holder, position);
        }
    }
}