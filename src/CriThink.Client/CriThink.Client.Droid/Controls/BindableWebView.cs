using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;

namespace CriThink.Client.Droid.Controls
{
    public class BindableWebView : WebView
    {
        protected BindableWebView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public BindableWebView(Context context)
            : base(context)
        { }

        public BindableWebView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        { }

        private Uri _uri;
        public Uri Uri
        {
            get => _uri;
            set
            {
                if (value is null)
                    return;

                _uri = value;

                LoadUrl(_uri.AbsoluteUri);
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            VerticalScrollBarEnabled = true;
            ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
        }
    }
}