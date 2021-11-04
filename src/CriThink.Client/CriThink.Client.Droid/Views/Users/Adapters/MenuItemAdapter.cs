using System;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.Models.Menu;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using static Android.Widget.TextView;

namespace CriThink.Client.Droid.Views.Users.Adapters
{
    public class MenuItemAdapter : MvxRecyclerAdapter
    {
        public MenuItemAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        { }

        [Preserve(Conditional = true)]
        protected MenuItemAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (Resource.Layout.cell_menu == viewType)
            {
                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
                var view = InflateViewForHolder(parent, Resource.Layout.cell_menu, itemBindingContext);

                return new MenuItemViewHolder(view, itemBindingContext);
            }
            else if (Resource.Layout.cell_footer == viewType)
            {

                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
                var view = InflateViewForHolder(parent, Resource.Layout.cell_footer, itemBindingContext);

                return new FooterItemViewHolder(view, itemBindingContext);
            }
            else if (Resource.Layout.cell_accent_menu == viewType)
            {

                var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
                var view = InflateViewForHolder(parent, Resource.Layout.cell_accent_menu, itemBindingContext);

                return new AccentMenuItemViewHolder(view, itemBindingContext);
            }
            else
            {
                return base.OnCreateViewHolder(parent, viewType);
            }
        }
    }

    public class MenuItemViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatImageView _imgMenu;
        private readonly AppCompatTextView _txtMenu;

        public MenuItemViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _imgMenu = itemView.FindViewById<AppCompatImageView>(Resource.Id.imgMenu);
            _txtMenu = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtMenu);
            this.DelayBind(() =>
            {
                using var set = this.CreateBindingSet<MenuItemViewHolder, MenuModel>();

                set.Bind(_txtMenu).For(v => v.Text).To(m => m.Text);
                set.Bind(_imgMenu).For(v => v.BindDrawableName()).To(m => m.IconPath);

                set.Bind(_txtMenu).For(v => v.BindClick()).To(m => m.Command);
                set.Bind(_imgMenu).For(v => v.BindClick()).To(m => m.Command);
            });
        }
    }

    public class AccentMenuItemViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatImageView _imgMenu;
        private readonly AppCompatTextView _txtMenu;

        public AccentMenuItemViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _imgMenu = itemView.FindViewById<AppCompatImageView>(Resource.Id.imgMenu);
            _txtMenu = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtMenu);

            this.DelayBind(() =>
            {
                using var set = this.CreateBindingSet<AccentMenuItemViewHolder, AccentMenuModel>();

                set.Bind(_txtMenu).For(v => v.Text).To(m => m.Text);
                set.Bind(_imgMenu).For(v => v.BindDrawableName()).To(m => m.IconPath);

                set.Bind(_txtMenu).For(v => v.BindClick()).To(m => m.Command);
                set.Bind(_imgMenu).For(v => v.BindClick()).To(m => m.Command);
            });
        }
    }

    public class FooterItemViewHolder : MvxRecyclerViewHolder
    {
        private readonly AppCompatTextView _txtMadeWithLoveOne;
        private readonly AppCompatTextView _txtMadeWithLoveTwo;

        public FooterItemViewHolder(View itemView, IMvxAndroidBindingContext context)
            : base(itemView, context)
        {
            _txtMadeWithLoveOne = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtMadeWithLove);
            _txtMadeWithLoveTwo = itemView.FindViewById<AppCompatTextView>(Resource.Id.txtMadeWithLoveTwo);
            var txtMadeWith = "Made with";
            var hearth = " ❤️ ";
            var last = "in Italy and United Kingdom";

            SpannableStringBuilder builderFirstRow = new SpannableStringBuilder();
            SpannableString spannableMadeWith = new SpannableString(txtMadeWith);
            builderFirstRow.Append(spannableMadeWith);

            SpannableString str2 = new SpannableString(hearth);
            str2.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Red), 0, str2.Length(), 0);
            builderFirstRow.Append(str2);

            _txtMadeWithLoveOne.SetText(builderFirstRow, BufferType.Spannable);
            _txtMadeWithLoveTwo.SetText(last, BufferType.Normal);
        }
    }
}
