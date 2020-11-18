using System;

#pragma warning disable CA1054 // URI-like parameters should not be strings
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

        /// <summary>
        /// Validates the given string as <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">Uri to validate</param>
        /// <returns>True if the given <see cref="Uri"/> is valid</returns>
        public static bool IsValidWebSite(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return false;

            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }
    }
}
