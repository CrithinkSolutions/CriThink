using System;

namespace CriThink.Server.Core.QueryResults
{
    public class GetAllTriggerLogQueryResult
    {
        public Guid Id { get; set; }

        public bool IsSuccessful { get; set; }

        public string TimeStamp { get; set; }

        public string FailReason { get; set; }
    }
}
