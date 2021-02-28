using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class SearchNewsSourceQuery : IRequest<SearchNewsSourceQueryResponse>
    {
        public SearchNewsSourceQuery(string newsLink)
        {
            NewsLink = newsLink;
        }

        public string NewsLink { get; }
    }
}
