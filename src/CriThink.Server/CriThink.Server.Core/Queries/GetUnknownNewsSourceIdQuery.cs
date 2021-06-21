using System;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetUnknownNewsSourceIdQuery : IRequest<Guid>
    {
        public GetUnknownNewsSourceIdQuery(string domain)
        {
            Domain = domain;
        }

        public string Domain { get; }
    }
}
