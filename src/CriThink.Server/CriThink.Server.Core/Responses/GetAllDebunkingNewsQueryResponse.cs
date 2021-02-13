﻿using System;

namespace CriThink.Server.Core.Responses
{
    public class GetAllDebunkingNewsQueryResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Publisher { get; set; }

        public string PublisherLanguage { get; set; }

        public string PublisherCountry { get; set; }

        public string NewsLink { get; set; }

        public string NewsImageLink { get; set; }
    }
}
