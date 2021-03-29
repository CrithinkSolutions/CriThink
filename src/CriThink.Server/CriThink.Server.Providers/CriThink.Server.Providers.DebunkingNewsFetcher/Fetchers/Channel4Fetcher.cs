using System;
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
    internal class Channel4Fetcher : BaseFetcher
    {
        private const string ImagePattern = "(?:<meta property=\"og:image\" content=\")(.+)(?:\"\\/>)";

        private readonly HttpClient _httpClient;
        private readonly ILogger<Channel4Fetcher> _logger;

        public Channel4Fetcher(IHttpClientFactory httpClientFactory, IOptions<Channel4Settings> options, ILogger<Channel4Fetcher> logger)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient(DebunkingNewsFetcherBootstrapper.Channel4HttpClientName);
            _logger = logger;

            WebSiteUri = options.Value.Uri;
        }

        internal static Uri WebSiteUri { get; private set; }

        public override Task<DebunkingNewsProviderResult>[] AnalyzeAsync()
        {
            var analysisTask = Task.Run(async () =>
            {
                Debug.WriteLine("Get in Channel4 fetcher");
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
                try
                {
                    var link = GetLink(item);
                    var imageUri = await GetNewsImageAsync(link).ConfigureAwait(false);

                    list.Add(new DebunkingNewsResponse(item.Title.Text, link,
                        imageUri,
                        item.PublishDate));
                }
                catch (LinkUnavailableException ex)
                {
                    _logger?.LogError(ex, $"Can't get link of Channel4 news {item.Title.Text}", item.Id);
                }
            }

#if DEBUG
            if (!list.Any())
            {
                list.Add(new DebunkingNewsResponse(
                    "Hancock suggests ‘no evidence’ UK variant is more severe",
                    "https://www.channel4.com/news/factcheck/factcheck-hancock-suggests-no-evidence-uk-variant-is-more-severe",
                    "https://fournews-assets-prod-s3-ew1-nmprod.s3.amazonaws.com/media/2021/02/shutterstock_editorial_11661756o-1920x1080.jpg",
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
