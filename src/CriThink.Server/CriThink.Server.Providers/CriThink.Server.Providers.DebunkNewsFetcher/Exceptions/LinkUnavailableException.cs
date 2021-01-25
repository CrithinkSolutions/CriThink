using System;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Exceptions
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class LinkUnavailableException : Exception
    {
        public LinkUnavailableException(string provider, string link) : base($"Unable to resolve link '{link}' from provider named '{provider}'") { }

        public LinkUnavailableException(string provider, string link, Exception innerException) : base($"Unable to resolve link '{link}' from provider named '{provider}'", innerException) { }

    }
}
