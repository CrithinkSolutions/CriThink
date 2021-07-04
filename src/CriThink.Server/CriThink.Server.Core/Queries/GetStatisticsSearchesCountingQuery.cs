using System;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetStatisticsSearchesCountingQuery : IRequest<GetStatisticsSearchesCountingQueryResponse>
    {
        public GetStatisticsSearchesCountingQuery()
        { }

        public GetStatisticsSearchesCountingQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid? UserId { get; }
    }
}
