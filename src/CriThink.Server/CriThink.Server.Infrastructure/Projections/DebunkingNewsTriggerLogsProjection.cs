using System;
using System.Globalization;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal class DebunkingNewsTriggerLogsProjection
    {
        /// <summary>
        /// Get a single item timestamp property
        /// </summary>
        internal static Expression<Func<DebunkingNewsTriggerLog, DateTime>> GetTimeStamp =>
            log => DateTime.Parse(log.TimeStamp, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
    }
}
