using System;
using System.Globalization;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Infrastructure.Projections
{
    internal class DebunkingNewsTriggerLogsProjection
    {
        /// <summary>
        /// Get a single item timestamp property
        /// </summary>
        internal static Expression<Func<DebunkingNewsTriggerLog, DateTime>> GetTimeStamp =>
            log => DateTime.Parse(log.TimeStamp, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        /// <summary>
        /// Get a single item getting all properties
        /// </summary>
        internal static Expression<Func<DebunkingNewsTriggerLog, GetAllTriggerLogQueryResponse>> GetAll =>
            log => new GetAllTriggerLogQueryResponse
            {
                Id = log.Id,
                IsSuccessful = log.IsSuccessful,
                TimeStamp = log.TimeStamp,
                FailReason = log.FailReason
            };
    }
}
