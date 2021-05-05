using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfile>
    {
        public GetUserProfileQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
