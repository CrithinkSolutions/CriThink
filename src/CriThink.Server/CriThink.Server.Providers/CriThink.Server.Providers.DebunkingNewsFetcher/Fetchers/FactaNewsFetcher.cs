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
    internal class FactaNewsFetcher : BaseFetcher
    {
        private const string ImagePattern = "<img[^>]+src=\"([^\"]+)\" class=\"attachment-full size-full wp-post-image\"";

        private readonly IDebunkingNewsPublisherService _debunkingNewsPublisherService;
        private readonly ITextAnalyticsService _textAnalyticsService;
        private readonly INewsScraperService _newsScraperService;
        private readonly Uri _webSiteUri;
        private readonly IReadOnlyList<string> _feedCategories;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FactaNewsFetcher> _logger;

        private DebunkingNewsPublisher _cachedDebunkingNewsPublisher;

        public FactaNewsFetcher(
            IHttpClientFactory httpClientFactory,
            IDebunkingNewsPublisherService debunkingNewsPublisherService,
            ITextAnalyticsService textAnalyticsService,
            INewsScraperService newsScraperService,
            IOptions<FactaNewsSettings> options,
            ILogger<FactaNewsFetcher> logger)
        {
            if (httpClientFactory is null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.FactaNewsHttpClientName);

            _debunkingNewsPublisherService = debunkingNewsPublisherService ??
                throw new ArgumentNullException(nameof(debunkingNewsPublisherService));

            _textAnalyticsService = textAnalyticsService ??
                throw new ArgumentNullException(nameof(_textAnalyticsService));

            _newsScraperService = newsScraperService ??
                throw new ArgumentNullException(nameof(newsScraperService));

            _logger = logger;

            _webSiteUri = options.Value.Uri;
            _feedCategories = new List<string>(options.Value.Categories);
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
                _logger?.LogCritical(ex, "Error getting Facta.News feed rss");
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
                    "QUESTA FEDINA PENALE DI GEORGE FLOYD CONTIENE DIVERSE IMPRECISIONI",
                    "https://facta.news/notizia-imprecisa/2021/06/24/questa-fedina-penale-di-george-floyd-contiene-diverse-imprecisioni/",
                    "https://facta.news/wp-content/uploads/2021/06/xFloyd.jpg.pagespeed.ic.7MQc0WD7NF.webp",
                    DateTime.Now);

                await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.FactaNews);

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
                var link = GetLink(item);
                var scrapedNews = await _newsScraperService.ScrapeNewsWebPage(new Uri(link));

                var imageUri = await GetNewsImageAsync(link);

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
                        await debunkingNews.SetPublisherAsync(_debunkingNewsPublisherService, EntityConstants.FactaNews);
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

                var keywords = await _textAnalyticsService.GetKeywordsFromTextAsync(scrapedNews.NewsBody, debunkingNews.Publisher.Language.Code);
                debunkingNews.SetKeywords(keywords);

                return new(debunkingNews);
            }
            catch (LinkUnavailableException ex)
            {
                _logger?.LogError(ex, $"Can't get link of FactaNews news {item.Title.Text}", item.Id);
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

            throw new LinkUnavailableException(DebunkingNewsFetcherBootstrapper.FactaNewsHttpClientName, item.Id);
        }

        private async Task<string> GetNewsImageAsync(string link)
        {
            var html = await _httpClient.GetStringAsync(link);

            var match = Regex.Match(html, ImagePattern);
            if (match.Groups is null || match.Groups.Count < 2)
            {
                _logger?.LogWarning($"Can't get image url of the following item: {link}; {html}");
                return string.Empty;
            }

            return match.Groups[1].Value;
        }
    }
}
