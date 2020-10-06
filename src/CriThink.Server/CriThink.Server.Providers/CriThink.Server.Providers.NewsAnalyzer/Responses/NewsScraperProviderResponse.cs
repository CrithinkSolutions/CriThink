using System;
using SmartReader;

// ReSharper disable CheckNamespace

namespace CriThink.Server.Providers.NewsAnalyzer
{
    /// <summary>
    /// Represents the news scraping response, providing the news info
    /// </summary>
    public class NewsScraperProviderResponse
    {
        public NewsScraperProviderResponse(Article article, Uri uri)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            Author = article.Author;
            Title = article.Title;
            Language = article.Language;
            TimeToRead = article.TimeToRead;
            Date = article.PublicationDate;
            NewsBody = article.TextContent;
            IsReadable = article.IsReadable;
            WebSiteName = article.SiteName;

            RequestedUri = uri;
        }

        /// <summary>
        /// News author
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// News title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// News language
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Time needed to read the news
        /// </summary>
        public TimeSpan TimeToRead { get; }

        /// <summary>
        /// News publishing date
        /// </summary>
        public DateTime? Date { get; }

        /// <summary>
        /// The content of the news
        /// </summary>
        public string NewsBody { get; }

        /// <summary>
        /// The name of the website
        /// </summary>
        public string WebSiteName { get; }

        /// <summary>
        /// True if the news has been successfully parsed
        /// </summary>
        public bool IsReadable { get; }

        /// <summary>
        /// The news uri
        /// </summary>
        public Uri RequestedUri { get; }

        /// <summary>
        /// Get the first 100 characters of the news body
        /// </summary>
        /// <returns></returns>
        public string GetCaption() => NewsBody.Length > 100 ? $"{NewsBody.Substring(0, 100)}..." : NewsBody;
    }
}
