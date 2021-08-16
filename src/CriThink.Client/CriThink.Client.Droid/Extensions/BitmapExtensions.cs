using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using static Android.Graphics.Bitmap;
using static Android.Graphics.PorterDuff;

namespace CriThink.Client.Droid.Extensions
{
    public static class BitmapExtensions
    {
        public static Bitmap RoundedCornerBitmap(this Bitmap bitmap, int size, int borderWidth, Color borderColor)
        {
            var padding = borderWidth * 2;
            
            var factor = Math.Min(bitmap.Height, bitmap.Width) / (double)(size + padding);
            int width = (int) (bitmap.Width / factor);
            int height = (int) (bitmap.Height / factor);
            size = Math.Min(width, height);
            bitmap = CreateScaledBitmap(bitmap, width, height, false);
            int radius = size / 2;
            var output = Bitmap.CreateBitmap(size + padding, size + padding, Config.Argb8888);
            var c = new Canvas(output);
            c.DrawARGB(0, 0, 0, 0);
            var p = new Paint();
            p.AntiAlias = true;
            p.SetStyle(Paint.Style.Fill);
            c.DrawCircle((size / 2) + borderWidth, (size / 2) + borderWidth, radius, p);
            p.SetXfermode(new PorterDuffXfermode(Mode.SrcIn));
            c.DrawBitmap(bitmap, ((size + padding)  / 2) - (width / 2) , padding, p);
            p.SetXfermode(null);
            p.SetStyle(Paint.Style.Stroke);
            p.Color = borderColor; 
            p.StrokeWidth = borderWidth;
            c.DrawCircle((size / 2) + borderWidth,  (size / 2) + borderWidth, radius, p);
            return output;
        }
    }
}
