using System;
using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllSubscribedUsersQuery : IRequest<IList<GetAllSubscribedUsersResponse>>
    {
        public GetAllSubscribedUsersQuery(Guid unknownNewsSourceId, int pageSize, int pageIndex)
        {
            UnknownNewsSourceId = unknownNewsSourceId;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public Guid UnknownNewsSourceId { get; }
        public int PageSize { get; }
        public int PageIndex { get; }
    }
}
