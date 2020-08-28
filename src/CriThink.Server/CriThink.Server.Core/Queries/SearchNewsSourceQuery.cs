using System;
using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class SearchNewsSourceQuery : IRequest<SearchNewsSourceQueryResponse>
    {
        public SearchNewsSourceQuery(Uri uri)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        public Uri Uri { get; }
    }
}
