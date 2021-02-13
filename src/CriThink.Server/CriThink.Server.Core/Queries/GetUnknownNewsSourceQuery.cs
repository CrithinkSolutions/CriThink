using System;
using CriThink.Server.Core.Entities;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetUnknownNewsSourceQuery : IRequest<UnknownNewsSource>
    {
        public GetUnknownNewsSourceQuery(Guid unknownNewsSourceId)
        {
            UnknownNewsSourceId = unknownNewsSourceId;
        }

        public Guid UnknownNewsSourceId { get; }
    }
}
