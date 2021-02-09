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
    }
}
