using System;
using System.Linq.Expressions;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownSourceNotificationRequestProjection
    {
        internal static Expression<Func<UnknownNewsSourceNotification, GetAllSubscribedUsersQueryResult>> GetAll =>
            unknownSourceNotificationRequest => new GetAllSubscribedUsersQueryResult
            {
                Id = unknownSourceNotificationRequest.Id,
                Email = unknownSourceNotificationRequest.Email,
            };

        internal static Expression<Func<UnknownNewsSourceNotification, GetAllSubscribedUsersWithSourceQueryResult>> GetAllWithSources =>
            unknownSourceNotificationRequest => new GetAllSubscribedUsersWithSourceQueryResult
            {
                Id = unknownSourceNotificationRequest.Id,
                Email = unknownSourceNotificationRequest.Email,
                RequestedAt = unknownSourceNotificationRequest.RequestedAt.ToString("u"),
                Domain = unknownSourceNotificationRequest.UnknownNewsSource.Uri,
                RequestCount = unknownSourceNotificationRequest.UnknownNewsSource.RequestCount,
            };
    }
}
