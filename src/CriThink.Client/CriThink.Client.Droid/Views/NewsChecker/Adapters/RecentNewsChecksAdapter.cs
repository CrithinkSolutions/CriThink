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
            if (Resource.Layout.cell_recentnewscheck_feed == viewType)
            {
                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
                var view = InflateViewForHolder(parent, Resource.Layout.cell_recentnewscheck_feed, itemBindingContext);

                return new RecentNewsCheckesViewHolder(view, itemBindingContext);
            }
            else if (Resource.Layout.cell_recentsearches_feed == viewType)
            {
                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
                var view = InflateViewForHolder(parent, Resource.Layout.cell_recentsearches_feed, itemBindingContext);

                return new RecentSearchesViewHolder(view, itemBindingContext);
            }
            else
            {
                return base.OnCreateViewHolder(parent, viewType);
            }
        }
    }

    public class RecentNewsCheckesViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _txtTitle;
        private readonly AppCompatTextView _txtLink;
        private readonly MvxCachedImageView _imgWebsite;
        private readonly AppCompatTextView _txtTimeStamp;

        public RecentNewsCheckesViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtTitle = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtTitle);
            _txtLink = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtLink);
            _imgWebsite = itemView.FindViewById<MvxCachedImageView>(Resource.Id.imgWebsite);
            _txtTimeStamp = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtTimeStamp);

            this.DelayBind(() =>
            {
                using var set = this.CreateBindingSet<RecentNewsCheckesViewHolder, RecentNewsChecksModel>();

                set.Bind(_txtTitle).To(vm => vm.Title);
                set.Bind(_imgWebsite).For(v => v.ImagePath).To(vm => vm.FavIcon);
                set.Bind(_txtLink).To(vm => vm.SearchedText);
                set.Bind(_txtTimeStamp).To(vm => vm.TimeStamp);

                set.Apply();
            });
        }
    }

    public class RecentSearchesViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _txtText;
        private readonly MvxCachedImageView _imgWebsite;
        private readonly AppCompatTextView _txtTimeStamp;

        public RecentSearchesViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtText = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtText);
            _imgWebsite = itemView.FindViewById<MvxCachedImageView>(Resource.Id.imgWebsite);
            _txtTimeStamp = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtTimeStamp);

            this.DelayBind(() =>
            {
                using var set = this.CreateBindingSet<RecentSearchesViewHolder, RecentNewsChecksModel>();

                set.Bind(_txtText).To(vm => vm.SearchedText);
                set.Bind(_imgWebsite).For(v => v.ImagePath).To(vm => vm.FavIcon);
                set.Bind(_txtTimeStamp).To(vm => vm.TimeStamp);

                set.Apply();
            });
        }
    }
}