using System;

namespace CriThink.Server.Core.QueryResults
{
    public class GetAllSubscribedUsersWithSourceQueryResult
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string RequestedAt { get; set; }

        public string Domain { get; set; }

        public int RequestCount { get; set; }
    }
}