using System;
using Com.Facebook.Shimmer;

namespace CriThink.Client.Droid.Extensions
{
    public static class ShimmerExtensions
    {
        public static void StartShimmerAnimation(this ShimmerFrameLayout shimmer, bool value)
        {
            if (value)
                shimmer.StartShimmer();
            else
                shimmer.StopShimmer();
        }
    }
}
