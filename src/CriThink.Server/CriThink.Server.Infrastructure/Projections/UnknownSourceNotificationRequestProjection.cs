using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownSourceNotificationRequestProjection
    {
        internal static Expression<Func<UnknownSourceNotificationRequest, UnknownSourceNotificationRequest>> GetAll =>
            unknownSourceNotificationRequest => new UnknownSourceNotificationRequest
            {
                Id = unknownSourceNotificationRequest.Id,
                Email = unknownSourceNotificationRequest.Email,
                RequestedAt = unknownSourceNotificationRequest.RequestedAt,
                UnknownNewsSourceId = unknownSourceNotificationRequest.UnknownNewsSourceId,
            };
    }
}
