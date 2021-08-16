using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using FFImageLoading;
using FFImageLoading.Drawables;
using FFImageLoading.Work;
using static Android.Graphics.Bitmap;
using static Android.Graphics.PorterDuff;

namespace CriThink.Client.Droid.Extensions
{
    public static class ImageServiceExtensions
    {

        public async static Task<SelfDisposingBitmapDrawable> AsBitmapRoundedDrawableAsync(this TaskParameter taskParameter, Context context, int size, int borderWidth, Color borderColor )
        {
            var bitmapDrawable = await taskParameter.AsBitmapDrawableAsync();
            return RoundedCornerBitmap(context, bitmapDrawable.Bitmap, size, borderWidth, borderColor);

        }


        private static SelfDisposingBitmapDrawable RoundedCornerBitmap(Context context, Bitmap bitmap, int size, int borderWidth, Color borderColor)
        {
            var output = bitmap.RoundedCornerBitmap(size, borderWidth, borderColor);
            return new SelfDisposingBitmapDrawable(context.Resources, output);
        }
    }
}
