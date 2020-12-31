using System;

namespace CriThink.Server.Core.Responses
{
    public class GetAllTriggerLogQueryResponse
    {
        public Guid Id { get; set; }

        public bool IsSuccessful { get; set; }

        public string TimeStamp { get; set; }

        public string FailReason { get; set; }
    }
}
