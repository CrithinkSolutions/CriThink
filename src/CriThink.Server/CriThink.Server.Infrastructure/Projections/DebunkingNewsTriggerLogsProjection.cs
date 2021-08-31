using System;
using System.Globalization;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Infrastructure.Projections
{
    public static class DebunkingNewsTriggerLogsProjection
    {
        /// <summary>
        /// Get a single item timestamp property
        /// </summary>
        public static Expression<Func<DebunkingNewsTriggerLog, DateTime>> GetTimeStamp =>
            log => DateTime.Parse(log.TimeStamp, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        /// <summary>
        /// Get a single item getting all properties
        /// </summary>
        internal static Expression<Func<DebunkingNewsTriggerLog, GetAllTriggerLogQueryResult>> GetAll =>
            log => new GetAllTriggerLogQueryResult
            {
                Id = log.Id,
                IsSuccessful = log.IsSuccessful,
                TimeStamp = log.TimeStamp,
                FailReason = log.FailReason
            };
    }
}
