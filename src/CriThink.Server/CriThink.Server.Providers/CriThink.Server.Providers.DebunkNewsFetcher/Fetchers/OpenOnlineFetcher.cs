using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using CriThink.Server.Providers.DebunkNewsFetcher.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace CriThink.Server.Providers.DebunkNewsFetcher.Fetchers
{
    internal class OpenOnlineFetcher : BaseFetcher
    {
        private const string ImagePattern = "(http|https):\\/\\/www.open.online\\/wp-content\\/uploads\\/\\d{4}\\/\\d{2}\\/(.+)\\.(\\w{2,4})";

        private readonly ILogger<OpenOnlineFetcher> _logger;
        private readonly HttpClient _httpClient;

        public OpenOnlineFetcher(IHttpClientFactory httpClientFactory, IOptions<WebSiteSettings> options, ILogger<OpenOnlineFetcher> logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.OpenOnlineHttpClientName);
            _logger = logger;

            FeedCategories = new List<string>(options.Value.Categories);
            WebSiteUri = options.Value.Uri;
        }

        internal static IReadOnlyList<string> FeedCategories { get; private set; }

        internal static Uri WebSiteUri { get; private set; }

        public override Task<DebunkingNewsProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(async () =>
            {
                Debug.WriteLine("Get in LanguageAnalyzer");
                return await RunFetcherAsync().ConfigureAwait(false);
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private async Task<DebunkingNewsProviderResult> RunFetcherAsync()
        {
            SyndicationFeed feed;

            try
            {
                feed = await GetSyndicationFeedAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new DebunkingNewsProviderResult(ex, $"Error getting feed: '{WebSiteUri}'");
            }

            var list = ReadFeed(feed);
            return new DebunkingNewsProviderResult(list);
        }

        private async Task<SyndicationFeed> GetSyndicationFeedAsync()
        {
            try
            {
                var result = await _httpClient.GetStreamAsync(WebSiteUri).ConfigureAwait(false);

                using var xmlReader = XmlReader.Create(result);
                var feed = SyndicationFeed.Load(xmlReader);
                return feed;
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error getting Open Online feed rss");
                throw;
            }
        }

        private IList<DebunkingNewsResponse> ReadFeed(SyndicationFeed feed)
        {
            var list = new List<DebunkingNewsResponse>();

            foreach (var item in feed.Items)
            {
                foreach (var categoryName in item.Categories.Select(c => c.Name.ToUpperInvariant()))
                {
                    if (!FeedCategories.Contains(categoryName)) continue;

                    var imageUri = GetNewsImageFromFeed(item);
                    list.Add(new DebunkingNewsResponse(item.Title.Text, item.Id, imageUri, item.PublishDate));
                    break;
                }
            }

#if DEBUG
            if (!list.Any())
            {
                list.Add(new DebunkingNewsResponse(
                    "Fondi Lega, arrestati tre commercialisti coinvolti nell’inchiesta su Lombardia Film Commission",
                    "https://www.open.online/?p=391373",
                    "https://www.open.online/wp-content/uploads/2020/02/guardia-di-finanza.jpg",
                    DateTime.Now));
            }
#endif
            return list;
        }

        private string GetNewsImageFromFeed(SyndicationItem item)
        {
            var htmlSnippet = item.Summary.Text;
            if (string.IsNullOrWhiteSpace(htmlSnippet))
            {
                _logger?.LogWarning($"Can't get image of the following item: {item.Id}");
                return string.Empty;
            }

            var match = Regex.Match(htmlSnippet, ImagePattern);
            if (string.IsNullOrWhiteSpace(match.Value))
                _logger?.LogWarning($"Can't get image url of the following item: {item.Id}; {htmlSnippet}");

            return match.Value;
        }
    }
}
