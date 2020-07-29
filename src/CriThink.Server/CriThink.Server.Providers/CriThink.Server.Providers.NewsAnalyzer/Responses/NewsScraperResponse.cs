using System;
using SmartReader;

namespace CriThink.Server.Providers.NewsAnalyzer.Responses
{
    /// <summary>
    /// Represents the result of a news scraping, providing the news info
    /// </summary>
    public class NewsScraperResponse
    {
        public NewsScraperResponse(Article article)
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
    }
}
