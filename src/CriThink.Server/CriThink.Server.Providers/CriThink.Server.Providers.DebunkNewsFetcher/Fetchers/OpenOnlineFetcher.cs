using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using CriThink.Server.Providers.DebunkNewsFetcher.Settings;
using Microsoft.Extensions.Options;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace CriThink.Server.Providers.DebunkNewsFetcher.Fetchers
{
    internal class OpenOnlineFetcher : BaseFetcher
    {
        private readonly HttpClient _httpClient;

        public OpenOnlineFetcher(IHttpClientFactory httpClientFactory, IOptions<WebSiteSettings> options)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.OpenOnlineHttpClientName);

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
            var list = new List<DebunkingNewsResponse>();

            try
            {
                var result = await _httpClient.GetStreamAsync(WebSiteUri).ConfigureAwait(false);

                using var xmlReader = XmlReader.Create(result);
                feed = SyndicationFeed.Load(xmlReader);
            }
            catch (Exception ex)
            {
                return new DebunkingNewsProviderResult(ex, $"Error getting feed: '{WebSiteUri}'");
            }

            if (feed != null)
            {
                foreach (var item in feed.Items)
                {
                    foreach (var categoryName in item.Categories.Select(c => c.Name.ToUpperInvariant()))
                    {
                        if (FeedCategories.Contains(categoryName))
                        {
                            list.Add(new DebunkingNewsResponse(item.Title.Text, item.Id, item.PublishDate));
                        }
                    }
                }
            }

#if DEBUG
            if (!list.Any())
            {
                list.Add(new DebunkingNewsResponse("Fondi Lega, arrestati tre commercialisti coinvolti nell’inchiesta su Lombardia Film Commission", "https://www.open.online/?p=391373", DateTime.Now));
            }
#endif

            return new DebunkingNewsProviderResult(list);
        }
    }
}
