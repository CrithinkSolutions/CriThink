using System;
using System.Linq.Expressions;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Infrastructure.Projections
{
    public static class DebunkingNewsTriggerLogsProjection
    {
        /// <summary>
        /// Get a single item timestamp property
        /// </summary>
        public static Expression<Func<DebunkingNewsTriggerLog, DateTimeOffset>> GetTimeStamp =>
            log => log.TimeStamp;

        /// <summary>
        /// Get a single item getting all properties
        /// </summary>
        internal static Expression<Func<DebunkingNewsTriggerLog, GetAllTriggerLogQueryResult>> GetAll =>
            log => new GetAllTriggerLogQueryResult
            {
                Id = log.Id,
                Status = log.Status,
                TimeStamp = log.TimeStamp.UtcDateTime,
                FailReason = log.Failures
            };
    }
}
