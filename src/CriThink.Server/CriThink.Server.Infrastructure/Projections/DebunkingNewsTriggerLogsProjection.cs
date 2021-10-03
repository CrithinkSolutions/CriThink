﻿using System;
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
        public static Expression<Func<DebunkingNewsTriggerLog, DateTime>> GetTimeStamp =>
            log => log.TimeStamp.UtcDateTime;

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
