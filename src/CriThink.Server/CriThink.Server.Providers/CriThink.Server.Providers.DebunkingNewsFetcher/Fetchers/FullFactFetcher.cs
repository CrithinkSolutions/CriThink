using System;
using System.Collections.Generic;
#if DEBUG
using System.Linq;
#endif
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using CriThink.Server.Providers.DebunkingNewsFetcher.Exceptions;
using CriThink.Server.Providers.DebunkingNewsFetcher.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers
{
    internal class FullFactFetcher : BaseFetcher
    {
        private static Uri WebSiteUri;

        private readonly HttpClient _httpClient;
        private readonly ILogger<FullFactFetcher> _logger;

        public FullFactFetcher(IHttpClientFactory httpClientFactory, IOptions<FullFactSettings> options, ILogger<FullFactFetcher> logger)
        {
            if (httpClientFactory is null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.FullFactHttpClientName);
            _logger = logger;

            WebSiteUri = options.Value.Uri;
        }

        public override Task<DebunkingNewsProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(async () =>
            {
                _logger?.LogInformation("FullFact fetcher is running");
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

            var list = await ReadFeedAsync(feed).ConfigureAwait(false);
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
                _logger?.LogCritical(ex, "Error getting FullFact feed rss");
                throw;
            }
        }

        private Task<List<DebunkingNewsResponse>> ReadFeedAsync(SyndicationFeed feed)
        {
            var list = new List<DebunkingNewsResponse>();

            foreach (var item in feed.Items)
            {
                try
                {
                    var link = GetLink(item);
                    list.Add(new DebunkingNewsResponse(item.Title.Text, item.Id, item.PublishDate));
                }
                catch (LinkUnavailableException ex)
                {
                    _logger?.LogError(ex, $"Can't get link of FullFact news {item.Title.Text}", item.Id);
                }
            }

#if DEBUG
            if (!list.Any())
            {
                list.Add(new DebunkingNewsResponse(
                    "Hydroxychloroquine study not all that it seems",
                    "https://fullfact.org/online/hydroxychloroquine-200-per-cent/",
                    string.Empty,
                    DateTime.Now));
            }
#endif

            return Task.FromResult(list);
        }

        private static string GetLink(SyndicationItem item)
        {
            var syndicationLink = item.Links.FirstOrDefault();

            if (syndicationLink is not null)
            {
                var link = syndicationLink.Uri.ToString();

                if (!string.IsNullOrEmpty(link))
                {
                    return link;
                }
            }

            throw new LinkUnavailableException(DebunkingNewsFetcherBootstrapper.Channel4HttpClientName, item.Id);
        }
    }
}
