using System;
using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllSubscribedUsersCommand : IRequest<IList<GetAllSubscribedUsersResponse>>
    {
        public GetAllSubscribedUsersCommand(Guid unknownNewsSourceId)
        {
            UnknownNewsSourceId = unknownNewsSourceId;
        }

        public Guid UnknownNewsSourceId { get; set; }
    }
}
