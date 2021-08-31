using System;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.QueryResults
{
    public class GetAllUnknownSourcesQueryResult
    {
        public Guid Id { get; set; }

        public string Domain { get; set; }

        public string RequestedAt { get; set; }

        public int RequestCount { get; set; }

        public NewsSourceAuthenticity NewsSourceAuthenticity { get; set; }

        public string IdentifiedAt { get; set; }
    }
}