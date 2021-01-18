using System;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Providers.DebunkNewsFetcher
{
    public class DebunkingNewsResponse
    {
        public DebunkingNewsResponse(string title, string link, string imageLink, DateTimeOffset publishingDate)
        {
            Title = title;
            Link = new Uri(link);
            ImageLink = imageLink;
            PublishingDate = publishingDate.DateTime;
        }

        public string Title { get; }

        public Uri Link { get; }

        public DateTime PublishingDate { get; }

        public string ImageLink { get; }
    }
}
