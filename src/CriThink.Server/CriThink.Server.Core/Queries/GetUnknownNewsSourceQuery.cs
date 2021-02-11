using System;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using MediatR;

namespace CriThink.Server.Core.Queries
{
    public class GetUnknownNewsSourceQuery : IRequest<UnknownNewsSourceResponse>
    {
        public GetUnknownNewsSourceQuery(Guid unknownNewsSourceId)
        {
            UnknownNewsSourceId = unknownNewsSourceId;
        }

        public Guid UnknownNewsSourceId { get; }
    }
}
