using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllTriggerLogsQuery : IRequest<IList<GetAllTriggerLogQueryResponse>>
    {
        public GetAllTriggerLogsQuery(int size, int index)
        {
            Size = size;
            Index = index;
        }

        public int Size { get; }

        public int Index { get; }
    }
}
