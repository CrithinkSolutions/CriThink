﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using CriThink.Server.Providers.DebunkingNewsFetcher.Exceptions;
using CriThink.Server.Providers.DebunkingNewsFetcher.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
namespace CriThink.Server.Providers.DebunkingNewsFetcher.Fetchers
{
    internal class OpenOnlineFetcher : BaseFetcher
    {
        private const string ImagePattern = "(http|https):\\/\\/www.open.online\\/wp-content\\/uploads\\/\\d{4}\\/\\d{2}\\/(.+)\\.(\\w{2,4})";

        private readonly ILogger<OpenOnlineFetcher> _logger;
        private readonly HttpClient _httpClient;
        private readonly HttpClient _urlResolver;

        public OpenOnlineFetcher(IHttpClientFactory httpClientFactory, IOptions<OpenOnlineSettings> options, ILogger<OpenOnlineFetcher> logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.OpenOnlineHttpClientName);
            _urlResolver = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.UrlResolverHttpClientName);
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
                _logger?.LogCritical(ex, "Error getting Open Online feed rss");
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
                    if (!FeedCategories.Contains(categoryName)) continue;

                    var imageUri = GetNewsImageFromFeed(item);

                    var link = await GetOpenLinkAsync(item).ConfigureAwait(false);

                    list.Add(new DebunkingNewsResponse(item.Title.Text, link, imageUri, item.PublishDate));
                    break;
                }
            }

#if DEBUG
            if (!list.Any())
            {
                list.Add(new DebunkingNewsResponse(
                    "Fondi Lega, arrestati tre commercialisti coinvolti nell’inchiesta su Lombardia Film Commission",
                    "https://www.open.online/2020/09/10/fondi-lega-arrestati-commercialisti-inchiesta-lombardia-film-commission/",
                    "https://www.open.online/wp-content/uploads/2020/02/guardia-di-finanza.jpg",
                    DateTime.Now));
            }
#endif
            return list;
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
            var result = await _urlResolver.GetAsync(new Uri(item.Id)).ConfigureAwait(false);

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