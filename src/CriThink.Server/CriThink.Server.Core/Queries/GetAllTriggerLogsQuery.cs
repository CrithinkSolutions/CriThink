using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    /// <summary>
    /// Creates the query to get all trigger logs paginated. Returns one item more than
    /// required to facilitate paging for the client
    /// </summary>
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
