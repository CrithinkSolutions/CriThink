using System;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetUnknownNewsSourceIdCommand : IRequest<Guid>
    {
        public GetUnknownNewsSourceIdCommand(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; set; }
    }
}
