using System;
using System.Collections.Generic;
#if DEBUG
using System.Linq;
#endif
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using CriThink.Server.Providers.DebunkingNewsFetcher.Exceptions;
using CriThink.Server.Providers.DebunkingNewsFetcher.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers
{
    internal class FactaNewsFetcher : BaseFetcher
    {
        private const string ImagePattern = "<img[^>]+src=\"([^\"]+)\" class=\"attachment-full size-full wp-post-image\"";

        private readonly Uri _webSiteUri;
        private readonly IReadOnlyList<string> _feedCategories;
        private readonly HttpClient _httpClient;
        private readonly ILogger<FactaNewsFetcher> _logger;

        public FactaNewsFetcher(IHttpClientFactory httpClientFactory, IOptions<FactaNewsSettings> options, ILogger<FactaNewsFetcher> logger)
        {
            if (httpClientFactory is null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.FactaNewsHttpClientName);
            _logger = logger;

            _webSiteUri = options.Value.Uri;
            _feedCategories = new List<string>(options.Value.Categories);
        }

        public override Task<DebunkingNewsProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(async () =>
            {
                _logger?.LogInformation("FactaNews fetcher is running");
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
                _logger?.LogCritical(ex, "Error getting Facta.News feed rss");
                throw;
            }
        }

        private async Task<IList<DebunkingNewsResponse>> ReadFeedAsync(SyndicationFeed feed)
        {
            var list = new List<DebunkingNewsResponse>();

            foreach (var item in feed.Items)
            {
                foreach (var categoryName in item.Categories.Select(c => c.Name.ToUpperInvariant()))
                {
                    if (!_feedCategories.Contains(categoryName)) continue;

                    try
                    {
                        var link = GetLink(item);
                        var imageUri = await GetNewsImageAsync(link).ConfigureAwait(false);

                        list.Add(new DebunkingNewsResponse(item.Title.Text, link, imageUri, item.PublishDate));
                        break;
                    }
                    catch (LinkUnavailableException ex)
                    {
                        _logger?.LogError(ex, $"Can't get link of FactaNews news {item.Title.Text}", item.Id);
                    }
                }
            }

#if DEBUG
            if (!list.Any())
            {
                list.Add(new DebunkingNewsResponse(
                    "QUESTA FEDINA PENALE DI GEORGE FLOYD CONTIENE DIVERSE IMPRECISIONI",
                    "https://facta.news/notizia-imprecisa/2021/06/24/questa-fedina-penale-di-george-floyd-contiene-diverse-imprecisioni/",
                    "https://facta.news/wp-content/uploads/2021/06/xFloyd.jpg.pagespeed.ic.7MQc0WD7NF.webp",
                    DateTime.Now));
            }
#endif

            return list;
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
            var html = await _httpClient.GetStringAsync(link).ConfigureAwait(false);

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
