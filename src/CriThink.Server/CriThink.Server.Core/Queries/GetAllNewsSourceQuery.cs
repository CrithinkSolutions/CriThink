using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllNewsSourceQuery : IRequest<IList<GetAllNewsSourceQueryResponse>>
    {
        public GetAllNewsSourceQuery(int size, int index, GetAllNewsSourceFilter filter)
        {
            Size = size;
            Index = index;
            Filter = filter;
        }

        public int Size { get; }

        public int Index { get; }

        public GetAllNewsSourceFilter Filter { get; }
    }

    public enum GetAllNewsSourceFilter
    {
        All,

        Blacklist,

        Whitelist
    }
}
