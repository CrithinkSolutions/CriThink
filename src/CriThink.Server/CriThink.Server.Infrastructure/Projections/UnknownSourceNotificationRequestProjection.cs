using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownSourceNotificationRequestProjection
    {
        internal static Expression<Func<UnknownNewsSourceNotificationRequest, GetAllSubscribedUsersResponse>> GetAll =>
            unknownSourceNotificationRequest => new GetAllSubscribedUsersResponse
            {
                Id = unknownSourceNotificationRequest.Id,
                Email = unknownSourceNotificationRequest.Email,
            };

        internal static Expression<Func<UnknownNewsSourceNotificationRequest, GetAllSubscribedUsersWithSourceResponse>> GetAllWithSources =>
            unknownSourceNotificationRequest => new GetAllSubscribedUsersWithSourceResponse
            {
                Id = unknownSourceNotificationRequest.Id,
                Email = unknownSourceNotificationRequest.Email,
                RequestedAt = unknownSourceNotificationRequest.RequestedAt.ToString("u"),
                Domain = unknownSourceNotificationRequest.UnknownNewsSource.Uri,
                RequestCount = unknownSourceNotificationRequest.UnknownNewsSource.RequestCount,
            };
    }
}
