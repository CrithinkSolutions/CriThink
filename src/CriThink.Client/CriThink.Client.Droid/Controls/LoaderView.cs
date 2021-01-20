using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views.Animations;
using CriThink.Client.Droid.Helpers;
using Felipecsl.GifImageViewLibrary;

namespace CriThink.Client.Droid.Controls
{
    public class LoaderView : GifImageView
    {
        protected LoaderView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Initialize();
        }

        public LoaderView(Context context)
            : base(context)
        {
            Initialize();
        }

        public LoaderView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        private void Initialize()
        {
            if (Context == null)
                return;

            var input = Resources?.OpenRawResource(Resource.Drawable.loader_indicator);

            byte[] bytes = BytesHelper.ConvertByteArray(input);

            SetBytes(bytes);
            ApplyRotation();

            StartAnimation();
        }

        private void ApplyRotation()
        {
            var rotation = new RotateAnimation(0, 360, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f)
            {
                Duration = 1200,
                Interpolator = new DecelerateInterpolator(1.25f),
                RepeatCount = Animation.Infinite
            };

            Animation = rotation;
        }
    }
}