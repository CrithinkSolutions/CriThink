using System;

namespace CriThink.Server.Core.QueryResults
{
    public class GetAllDebunkingNewsByKeywordsQueryResult
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string PublisherName { get; set; }

        public string NewsLink { get; set; }

        public string NewsImageLink { get; set; }

        public string PublishingDate { get; set; }

        public string NewsCaption { get; set; }
    }
}