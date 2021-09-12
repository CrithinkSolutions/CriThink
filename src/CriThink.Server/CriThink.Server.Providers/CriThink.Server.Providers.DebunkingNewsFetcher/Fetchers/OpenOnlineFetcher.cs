using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading;
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
    internal class OpenOnlineFetcher : BaseFetcher
    {
        private const string ImagePattern = "(http|https):\\/\\/www.open.online\\/wp-content\\/uploads\\/\\d{4}\\/\\d{2}\\/(.+)\\.(\\w{2,4})";

        private readonly IDebunkingNewsPublisherService _debunkingNewsPublisherService;
        private readonly ITextAnalyticsService _textAnalyticsService;
        private readonly INewsScraperService _newsScraperService;
        private readonly ILogger<OpenOnlineFetcher> _logger;
        private readonly IReadOnlyList<string> _feedCategories;
        private readonly Uri _webSiteUri;
        private readonly HttpClient _httpClient;
        private readonly HttpClient _urlResolver;

        private DebunkingNewsPublisher _cachedDebunkingNewsPublisher;

        public OpenOnlineFetcher(
            IHttpClientFactory httpClientFactory,
            IDebunkingNewsPublisherService debunkingNewsPublisherService,
            ITextAnalyticsService textAnalyticsService,
            INewsScraperService newsScraperService,
            IOptions<OpenOnlineSettings> options,
            ILogger<OpenOnlineFetcher> logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.OpenOnlineHttpClientName);
            _urlResolver = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.UrlResolverHttpClientName);

            _debunkingNewsPublisherService = debunkingNewsPublisherService ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherService));

            _textAnalyticsService = textAnalyticsService ??
                throw new ArgumentNullException(nameof(_textAnalyticsService));

            _newsScraperService = newsScraperService ??
                throw new ArgumentNullException(nameof(newsScraperService));

            _logger = logger;

            _feedCategories = new List<string>(options.Value.Categories);
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
                feed = await GetSyndicationFeedAsync();
            }
            catch (Exception ex)
            {
                return new DebunkingNewsProviderResult(ex, $"Error getting feed: '{_webSiteUri}'");
            }

            var list = await ReadFeedAsync(feed);

            return new DebunkingNewsProviderResult(list);
        }

        private async Task<SyndicationFeed> GetSyndicationFeedAsync()
        {
            try
            {
                var result = await _httpClient.GetStreamAsync(_webSiteUri);

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

        private async Task<IList<Monad<DebunkingNews>>> ReadFeedAsync(SyndicationFeed feed)
        {
            var list = new List<Monad<DebunkingNews>>();

            foreach (var item in feed.Items.Where(i => i.PublishDate.DateTime > LastFetchingTimeStamp))
            {
                await ReadCategoriesAsync(list, item);
            }

#if DEBUG
            if (!list.Any())
            {
                var debunkingNews = DebunkingNews.Create(
                    "Fondi Lega, arrestati tre commercialisti coinvolti nell’inchiesta su Lombardia Film Commission",
                    "https://www.open.online/2020/09/10/fondi-lega-arrestati-commercialisti-inchiesta-lombardia-film-commission/",
                    "https://www.open.online/wp-content/uploads/2020/02/guardia-di-finanza.jpg",
                    DateTime.Now);

                await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.OpenOnline);

                list.Add(new(debunkingNews));
            }
#endif
            return list;
        }

        private async Task ReadCategoriesAsync(IList<Monad<DebunkingNews>> debunkingNewsCollector, SyndicationItem item)
        {
            foreach (var categoryName in item.Categories.Select(c => c.Name.ToUpperInvariant()))
            {
                if (!_feedCategories.Contains(categoryName))
                    continue;

                var debunkingNews = await ReadItemAsync(item);
                debunkingNewsCollector.Add(debunkingNews);
                break;
            }
        }

        private async Task<Monad<DebunkingNews>> ReadItemAsync(SyndicationItem item)
        {
            try
            {
                var link = await GetOpenLinkAsync(item);
                var scrapedNews = await _newsScraperService.ScrapeNewsWebPage(new Uri(link));

                var imageUri = GetNewsImageFromFeed(item);

                var debunkingNews = DebunkingNews.Create(
                    item.Title.Text,
                    link,
                    imageUri,
                    item.PublishDate);

                if (_cachedDebunkingNewsPublisher is null)
                {
                    await SemaphoreSlim.WaitAsync();

                    try
                    {
                        await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.OpenOnline);
                        _cachedDebunkingNewsPublisher = debunkingNews.Publisher;
                    }
                    finally
                    {
                        SemaphoreSlim.Release();
                    }
                }
                else
                {
                    debunkingNews.SetPublisher(_cachedDebunkingNewsPublisher);
                }

                var keywords = await _textAnalyticsService.GetKeywordsFromTextAsync(
                    scrapedNews.NewsBody,
                    debunkingNews.Publisher.Language.Code);

                debunkingNews.SetKeywords(keywords);

                return new(debunkingNews);
            }
            catch (LinkUnavailableException ex)
            {
                _logger?.LogError(ex, $"Can't get link of OpenOnline news {item.Title.Text}", item.Id);
                return new(ex);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        private async Task<string> GetOpenLinkAsync(SyndicationItem item)
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

            // Solve Redirect from "permalink"
            var result = await _urlResolver.GetAsync(new Uri(item.Id));

            if (result.StatusCode == System.Net.HttpStatusCode.Moved)
            {
                if (result.Headers.Location is not null)
                {
                    return result.Headers.Location.ToString();
                }
            }

            throw new LinkUnavailableException(DebunkingNewsFetcherBootstrapper.OpenOnlineHttpClientName, item.Id);
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
