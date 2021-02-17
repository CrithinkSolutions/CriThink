using System.Collections.Generic;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetAllDebunkingNewsByKeywordsQuery : IRequest<IList<GetAllDebunkingNewsByKeywordsQueryResponse>>
    {
        public GetAllDebunkingNewsByKeywordsQuery(IEnumerable<string> keywords)
        {
            Keywords = new List<string>(keywords).AsReadOnly();
        }

        public IReadOnlyList<string> Keywords { get; set; }
    }
}
