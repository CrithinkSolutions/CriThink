using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using CriThink.Server.Core.DomainServices;
using CriThink.Server.Core.Entities;
using CriThink.Server.Providers.DebunkingNewsFetcher.Exceptions;
using CriThink.Server.Providers.DebunkingNewsFetcher.Settings;
using CriThink.Server.Providers.NewsAnalyzer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers
{
    internal class FullFactFetcher : BaseFetcher
    {
        private static Uri WebSiteUri;

        private readonly HttpClient _httpClient;
        private readonly IDebunkingNewsPublisherService _debunkingNewsPublisherService;
        private readonly ITextAnalyticsService _textAnalyticsService;
        private readonly INewsScraperService _newsScraperService;
        private readonly ILogger<FullFactFetcher> _logger;

        private DebunkingNewsPublisher _cachedDebunkingNewsPublisher;

        public FullFactFetcher(
            IHttpClientFactory httpClientFactory,
            IDebunkingNewsPublisherService debunkingNewsPublisherService,
            ITextAnalyticsService textAnalyticsService,
            INewsScraperService newsScraperService,
            IOptions<FullFactSettings> options,
            ILogger<FullFactFetcher> logger)
        {
            if (httpClientFactory is null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.FullFactHttpClientName);

            _debunkingNewsPublisherService = debunkingNewsPublisherService ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherService));

            _textAnalyticsService = textAnalyticsService ??
                throw new ArgumentNullException(nameof(_textAnalyticsService));

            _newsScraperService = newsScraperService ??
                throw new ArgumentNullException(nameof(newsScraperService));

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

            var list = await ReadFeedAsync(feed);
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

        private async Task<IList<DebunkingNews>> ReadFeedAsync(SyndicationFeed feed)
        {
            var list = new List<DebunkingNews>();

            foreach (var item in feed.Items.Where(i => i.PublishDate.DateTime > LastFetchingTimeStamp))
            {
                try
                {
                    var debunkingNews = await ReadItemAsync(item);
                    list.Add(debunkingNews);
                }
                catch (LinkUnavailableException ex)
                {
                    _logger?.LogError(ex, $"Can't get link of FullFact news {item.Title.Text}", item.Id);
                }
            }

#if DEBUG
            if (!list.Any())
            {
                var debunkingNews = DebunkingNews.Create(
                    "Hydroxychloroquine study not all that it seems",
                    "https://fullfact.org/online/hydroxychloroquine-200-per-cent/",
                    string.Empty,
                    DateTime.Now);

                await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.FullFact);

                list.Add(debunkingNews);
            }
#endif

            return list;
        }

        private async Task<DebunkingNews> ReadItemAsync(SyndicationItem item)
        {
            var link = GetLink(item);

            var scrapedNews = await _newsScraperService.ScrapeNewsWebPage(new Uri(link));

            var debunkingNews = DebunkingNews.Create(
               item.Title.Text,
               link,
               item.PublishDate);

            if (_cachedDebunkingNewsPublisher is null)
            {
                await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.FullFact);
                _cachedDebunkingNewsPublisher = debunkingNews.Publisher;
            }
            else
            {
                debunkingNews.SetPublisher(_cachedDebunkingNewsPublisher);
            }

            var keywords = await _textAnalyticsService.GetKeywordsFromTextAsync(scrapedNews.NewsBody, debunkingNews.Publisher.Language.Code);
            debunkingNews.SetKeywords(keywords);

            return debunkingNews;
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
