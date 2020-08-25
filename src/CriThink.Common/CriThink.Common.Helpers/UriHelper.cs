using System;

namespace CriThink.Common.Helpers
{
    public static class UriHelper
    {
        /// <summary>
        /// Get the hostname from the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">Uri to get host from</param>
        /// <returns>Host in the following format: www.hostname.com/</returns>
        public static string GetHostNameFromUri(Uri uri)
        {
            if (uri == null)
                return string.Empty;

            return $"{uri.Host}/";
        }
    }
}
