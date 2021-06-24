using System;
using System.Collections.Generic;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Settings
{
#pragma warning disable CA2227 // Collection properties should be read only

    public class DebunkingNewsProviderSettings
    {
        public Uri Uri { get; set; }

        public List<string> Categories { get; set; }
    }

    public class OpenOnlineSettings : DebunkingNewsProviderSettings
    { }

    public class Channel4Settings : DebunkingNewsProviderSettings
    { }

    public class FactaNewsSettings : DebunkingNewsProviderSettings
    { }
}
