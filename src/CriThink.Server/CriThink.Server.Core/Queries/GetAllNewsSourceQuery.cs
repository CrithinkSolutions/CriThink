using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllNewsSourceQuery : IRequest<IEnumerable<GetAllNewsSourceQueryResponse>>, IRequest<Unit>
    {
        public GetAllNewsSourceQuery(GetAllNewsSourceFilter filter)
        {
            Filter = filter;
        }

        public GetAllNewsSourceFilter Filter { get; }
    }

    public enum GetAllNewsSourceFilter
    {
        All,

        Blacklist,

        Whitelist
    }
}
