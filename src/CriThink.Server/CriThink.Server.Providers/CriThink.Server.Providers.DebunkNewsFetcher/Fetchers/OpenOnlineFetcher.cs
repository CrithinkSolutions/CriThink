using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace CriThink.Server.Providers.DebunkNewsFetcher.Fetchers
{
    internal class OpenOnlineFetcher : BaseFetcher
    {
        private readonly HttpClient _httpClient;

        static OpenOnlineFetcher()
        {
            FeedCategories = new List<string>();
        }

        public OpenOnlineFetcher(ConcurrentQueue<Task<DebunkNewsProviderResult>> queue, IHttpClientFactory httpClientFactory)
            : base(queue)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient(DebunkNewsFetcherBootstrapper.OpenOnlineHttpClientName);
        }

        internal static List<string> FeedCategories { get; }
        internal static Uri WebSiteUri { get; set; }

        public override Task<DebunkNewsProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(async () =>
            {
                Debug.WriteLine("Get in LanguageAnalyzer");
                return await RunFetcherAsync().ConfigureAwait(false);
            });

            Queue.Enqueue(analysisTask);
            return base.AnalyzeAsync();
        }

        private async Task<DebunkNewsProviderResult> RunFetcherAsync()
        {
            SyndicationFeed feed;
            var list = new List<DebunkNewsResponse>();

            try
            {
                var result = await _httpClient.GetStreamAsync(WebSiteUri).ConfigureAwait(false);

                using var xmlReader = XmlReader.Create(result);
                feed = SyndicationFeed.Load(xmlReader);
            }
            catch (Exception ex)
            {
                return new DebunkNewsProviderResult(ex, $"Error getting feed: '{WebSiteUri}'");
            }

            if (feed != null)
            {
                foreach (var item in feed.Items)
                {
                    foreach (var categoryName in item.Categories.Select(c => c.Name.ToUpperInvariant()))
                    {
                        if (FeedCategories.Contains(categoryName))
                        {
                            list.Add(new DebunkNewsResponse(item.Title.Text, item.Id));
                        }
                    }
                }
            }

#if DEBUG
            if (!list.Any())
            {
                list.Add(new DebunkNewsResponse("Fondi Lega, arrestati tre commercialisti coinvolti nell’inchiesta su Lombardia Film Commission", "https://www.open.online/?p=391373"));
            }
#endif

            return new DebunkNewsProviderResult(list);
        }
    }
}
