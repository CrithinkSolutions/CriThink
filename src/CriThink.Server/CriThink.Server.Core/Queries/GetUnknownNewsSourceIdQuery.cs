using System;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetUnknownNewsSourceIdQuery : IRequest<Guid>
    {
        public GetUnknownNewsSourceIdQuery(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; }
    }
}
