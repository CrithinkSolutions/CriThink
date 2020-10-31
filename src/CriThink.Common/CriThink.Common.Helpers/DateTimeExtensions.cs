using System;

namespace CriThink.Common.Helpers
{
    public static class DateTimeExtensions
    {
        private static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTimestamp(this DateTime self) => (long)(self - Epoch).TotalSeconds;

        public static DateTime AsUnixTimestamp(this long self) => Epoch.AddSeconds(self);
    }
}