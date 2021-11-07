using System;
using Android.Views;

namespace CriThink.Client.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void VisibleOrGone(this View view, bool visible)
        {
            if (visible)
            {
                view.Visibility = ViewStates.Visible;
            }
            else
            {
                view.Visibility = ViewStates.Gone;
            }
        }
    }
}
