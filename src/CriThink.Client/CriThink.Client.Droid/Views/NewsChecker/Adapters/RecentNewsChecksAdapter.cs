using System;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.Models.NewsChecker;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;


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

        public RecentNewsCheckesViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtLink = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtLink);
            _imgWebsite = itemView.FindViewById<MvxCachedImageView>(Resource.Id.imgWebsite);

            this.DelayBind(() =>
            {
                using (var set = this.CreateBindingSet<RecentNewsCheckesViewHolder, RecentNewsChecksModel>())
                {
                    set.Bind(_txtLink).To(vm => vm.NewsLink);
                }
            });
        }
    }
}