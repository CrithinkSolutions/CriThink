using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using FFImageLoading;
using FFImageLoading.Cross;

namespace CriThink.Client.Droid.Controls
{
    public class LoaderView : MvxCachedImageView
    {
        private Animation _animation;

        protected LoaderView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public LoaderView(Context context)
            : base(context)
        { }

        public LoaderView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        { }

        protected override IParcelable OnSaveInstanceState()
        {
            Bundle bundle = new Bundle();
            bundle.PutParcelable("superState", base.OnSaveInstanceState());
            bundle.PutInt("visibility", (int) this.Visibility);
            return bundle;
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            if (state is Bundle bundle)
            {
                Visibility = (ViewStates) bundle.GetInt("visibility");
                state = (IParcelable) bundle.GetParcelable("superState");
            }

            base.OnRestoreInstanceState(state);
        }

        protected override void OnVisibilityChanged(View changedView, ViewStates visibility)
        {
            base.OnVisibilityChanged(changedView, visibility);

            if (visibility == ViewStates.Visible)
                StartAnimation(_animation);
            else
                ClearAnimation();
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Create();
        }

        public override void OnVisibilityAggregated(bool isVisible)
        {
            Visibility = isVisible ?
                ViewStates.Visible
                : ViewStates.Gone;
        }

        private void Create()
        {
            if (Context == null)
                return;

            ImageService.Instance.LoadCompiledResource("loader_indicator")
                .Into(this);

            _animation = ApplyRotation();
        }

        private static RotateAnimation ApplyRotation() =>
            new RotateAnimation(
                0,
                360,
                Dimension.RelativeToSelf,
                0.5f,
                Dimension.RelativeToSelf,
                0.5f)
            {
                Duration = 1200,
                Interpolator = new DecelerateInterpolator(1.25f),
                RepeatCount = Animation.Infinite
            };
    }
}