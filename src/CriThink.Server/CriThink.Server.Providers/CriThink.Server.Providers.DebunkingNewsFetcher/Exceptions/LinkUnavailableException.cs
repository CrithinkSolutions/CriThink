using System;
using System.Globalization;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Exceptions
{
    public class LinkUnavailableException : Exception
    {
        private const string ErrorMessage = "Unable to resolve link '{0}' from provider named '{1}'";

        public LinkUnavailableException(string provider, string link) : base(string.Format(CultureInfo.InvariantCulture, ErrorMessage, provider, link)) { }

        public LinkUnavailableException(string provider, string link, Exception innerException) : base(string.Format(CultureInfo.InvariantCulture, ErrorMessage, provider, link), innerException) { }

    }
}
