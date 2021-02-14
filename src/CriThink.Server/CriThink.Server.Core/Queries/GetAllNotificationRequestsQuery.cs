﻿using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    /// <summary>
    /// Creates the query to get all notification requests paginated. Returns one item more than
    /// required to facilitate paging for the client
    /// </summary>
    public class GetAllNotificationRequestsQuery : IRequest<List<GetAllSubscribedUsersWithSourceResponse>>
    {
        public GetAllNotificationRequestsQuery(int size, int index)
        {
            Size = size;
            Index = index;
        }

        public int Size { get; }

        public int Index { get; }
    }
}
