using System;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Domain.QueryResults
{
    public class GetAllTriggerLogQueryResult
    {
        public Guid Id { get; set; }

        public DebunkingNewsTriggerLogStatus Status { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string FailReason { get; set; }
    }
}
