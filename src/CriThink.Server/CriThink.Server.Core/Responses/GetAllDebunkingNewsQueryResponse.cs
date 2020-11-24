using System;

namespace CriThink.Server.Core.Responses
{
    public class GetAllDebunkingNewsQueryResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Publisher { get; set; }
    }
}
