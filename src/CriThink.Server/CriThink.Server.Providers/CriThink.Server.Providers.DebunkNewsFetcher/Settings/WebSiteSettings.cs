using System;
using System.Collections.Generic;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Settings
{
#pragma warning disable CA2227 // Collection properties should be read only

    public class WebSiteSettings
    {
        public Uri Uri { get; set; }

        public List<string> Categories { get; set; }
    }
}
