using System;
using Android.Runtime;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Common.Endpoints.DTOs.Admin;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace CriThink.Client.Droid.Views.DebunkingNews.Adapters
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

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var txtTitle = holder.ItemView.FindViewById<AppCompatTextView>(Resource.Id.txtTitle);

            if (GetItem(position) is DebunkingNewsGetResponse debunkingNews && txtTitle != null)
            {
                txtTitle.Text = debunkingNews.Title;
            }

            base.OnBindViewHolder(holder, position);
        }
    }
}