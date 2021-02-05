using System;
using MediatR;

namespace CriThink.Server.Core.Commands
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
