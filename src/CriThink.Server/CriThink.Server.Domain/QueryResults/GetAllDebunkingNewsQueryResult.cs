using System;

namespace CriThink.Server.Domain.QueryResults
{
    public class GetAllDebunkingNewsQueryResult
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Publisher { get; set; }

        public string PublisherLanguage { get; set; }

        public string PublisherCountry { get; set; }

        public string NewsLink { get; set; }

        public string NewsImageLink { get; set; }

        public string NewsCaption { get; set; }
    }
}
