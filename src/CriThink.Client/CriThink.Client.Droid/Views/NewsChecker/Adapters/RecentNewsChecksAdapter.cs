using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.Models.NewsChecker;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views;


// ReSharper disable once CheckNamespace
namespace CriThink.Client.Droid.Views.NewsChecker
{
    public class RecentNewsChecksAdapter : MvxRecyclerAdapter
    {
        public RecentNewsChecksAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        { }

        [Preserve(Conditional = true)]
        protected RecentNewsChecksAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, Resource.Layout.cell_recentnewscheck_feed, itemBindingContext);

            return new RecentNewsCheckesViewHolder(view, itemBindingContext);
        }
    }

    public class RecentNewsCheckesViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _txtLink;
        private readonly MvxCachedImageView _imgWebsite;
        private readonly AppCompatImageButton _btnHistoryDelete;
        public RecentNewsCheckesViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtLink = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtLink);
            _imgWebsite = itemView.FindViewById<MvxCachedImageView>(Resource.Id.imgWebsite);
            _btnHistoryDelete = itemView.FindViewById<AppCompatImageButton>(Resource.Id.btnHistoryDelete);
            this.DelayBind(() =>
            {
                using (var set = this.CreateBindingSet<RecentNewsCheckesViewHolder, RecentNewsChecksModel>())
                {
                    set.Bind(_txtLink).To(vm => vm.NewsLink);
                    set.Bind(_imgWebsite).For(v => v.ImagePath).To(vm => vm.NewsImageLink);
                    set.Bind(_btnHistoryDelete).For(v => v.BindClick()).To(vm => vm.DeleteHistoryRecentNewsItemCommand);
                }
            });
        }
    }
}