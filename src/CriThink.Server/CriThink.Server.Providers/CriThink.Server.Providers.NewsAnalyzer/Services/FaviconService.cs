using System.Globalization;

namespace CriThink.Server.Providers.NewsAnalyzer.Services
{
    internal class FaviconService : IFaviconService
    {
        private const string GoogleFavIconService = "https://www.google.com/s2/favicons?domain_url={0}";

        public string GetFaviconFromWebsite(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return null;

            return string.Format(CultureInfo.InvariantCulture, GoogleFavIconService, domain.ToLowerInvariant());
        }
    }
}
