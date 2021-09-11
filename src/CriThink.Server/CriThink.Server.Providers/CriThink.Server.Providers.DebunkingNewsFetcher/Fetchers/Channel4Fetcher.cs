using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using CriThink.Common.Helpers;
using CriThink.Server.Domain.DomainServices;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Providers.DebunkingNewsFetcher.Exceptions;
using CriThink.Server.Providers.DebunkingNewsFetcher.Settings;
using CriThink.Server.Providers.NewsAnalyzer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers
{
    internal class Channel4Fetcher : BaseFetcher
    {
        private const string ImagePattern = "(?:<meta property=\"og:image\" content=\")(.+)(?:\"\\/>)";

        private readonly HttpClient _httpClient;
        private readonly IDebunkingNewsPublisherService _debunkingNewsPublisherService;
        private readonly ITextAnalyticsService _textAnalyticsService;
        private readonly INewsScraperService _newsScraperService;
        private readonly ILogger<Channel4Fetcher> _logger;
        private readonly Uri _webSiteUri;

        private DebunkingNewsPublisher _cachedDebunkingNewsPublisher;

        public Channel4Fetcher(
            IHttpClientFactory httpClientFactory,
            IDebunkingNewsPublisherService debunkingNewsPublisherService,
            ITextAnalyticsService textAnalyticsService,
            INewsScraperService newsScraperService,
            IOptions<Channel4Settings> options,
            ILogger<Channel4Fetcher> logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.Channel4HttpClientName);

            _debunkingNewsPublisherService = debunkingNewsPublisherService ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherService));

            _textAnalyticsService = textAnalyticsService ??
                throw new ArgumentNullException(nameof(_textAnalyticsService));

            _newsScraperService = newsScraperService ??
                throw new ArgumentNullException(nameof(newsScraperService));

            _logger = logger;

            _webSiteUri = options.Value.Uri;
        }

        public override Task<DebunkingNewsProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(RunFetcherAsync);

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
                return new DebunkingNewsProviderResult(ex, $"Error getting feed: '{_webSiteUri}'");
            }

            var list = await ReadFeedAsync(feed).ConfigureAwait(false);

            return new DebunkingNewsProviderResult(list);
        }

        private async Task<SyndicationFeed> GetSyndicationFeedAsync()
        {
            try
            {
                var result = await _httpClient.GetStreamAsync(_webSiteUri).ConfigureAwait(false);

                using var xmlReader = XmlReader.Create(result);
                var feed = SyndicationFeed.Load(xmlReader);
                return feed;
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error getting Channel4 feed rss");
                throw;
            }
        }

        private async Task<IList<Monad<DebunkingNews>>> ReadFeedAsync(SyndicationFeed feed)
        {
            var list = new List<Monad<DebunkingNews>>();

            foreach (var item in feed.Items.Where(i => i.PublishDate.DateTime > LastFetchingTimeStamp))
            {
                var debunkingNews = await ReadItemAsync(item);
                list.Add(debunkingNews);
            }

#if DEBUG
            if (!list.Any())
            {
                var debunkingNews = DebunkingNews.Create(
                    "Hancock suggests ‘no evidence’ UK variant is more severe",
                    "https://www.channel4.com/news/factcheck/factcheck-hancock-suggests-no-evidence-uk-variant-is-more-severe",
                    "https://fournews-assets-prod-s3-ew1-nmprod.s3.amazonaws.com/media/2021/02/shutterstock_editorial_11661756o-1920x1080.jpg",
                    DateTime.Now);

                await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.Channel4);

                list.Add(new(debunkingNews));
            }
#endif

            return list;
        }

        private async Task<Monad<DebunkingNews>> ReadItemAsync(SyndicationItem item)
        {
            try
            {
                var link = GetLink(item);
                var scrapedNews = await _newsScraperService.ScrapeNewsWebPage(new Uri(link));

                var imageUri = await GetNewsImageAsync(link).ConfigureAwait(false);

                var debunkingNews = DebunkingNews.Create(
                    item.Title.Text,
                    link,
                    imageUri,
                    item.PublishDate);

                if (_cachedDebunkingNewsPublisher is null)
                {
                    await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.Channel4);
                    _cachedDebunkingNewsPublisher = debunkingNews.Publisher;
                }
                else
                {
                    debunkingNews.SetPublisher(_cachedDebunkingNewsPublisher);
                }

                var keywords = await _textAnalyticsService.GetKeywordsFromTextAsync(scrapedNews.NewsBody, debunkingNews.Publisher.Language.Code);
                debunkingNews.SetKeywords(keywords);

                return new(debunkingNews);
            }
            catch (LinkUnavailableException ex)
            {
                _logger?.LogError(ex, $"Can't get link of Channel4 news {item.Title.Text}", item.Id);
                return new(ex);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
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

        private async Task<string> GetNewsImageAsync(string link)
        {
            var html = await _httpClient.GetStringAsync(link).ConfigureAwait(false);

            var match = Regex.Match(html, ImagePattern);
            if (match.Groups.Count > 1 && match.Groups[1].Success)
                return match.Groups[1].Value;

            _logger?.LogWarning($"Can't get image url of the following item: {link}; {html}");
            return string.Empty;
        }
    }
}
