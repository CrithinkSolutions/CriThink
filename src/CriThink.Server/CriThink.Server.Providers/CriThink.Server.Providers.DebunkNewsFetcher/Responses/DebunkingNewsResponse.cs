using System;

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Providers.DebunkNewsFetcher
{
    public class DebunkingNewsResponse
    {
        public DebunkingNewsResponse(string title, string link, DateTimeOffset publishingDate)
        {
            Title = title;
            Link = link;
            PublishingDate = publishingDate.Date;
        }

        public string Title { get; }

        public string Link { get; }

        public DateTime PublishingDate { get; }
    }
}
