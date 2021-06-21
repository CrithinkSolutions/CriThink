using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownNewsSourceProjection
    {
        internal static Expression<Func<UnknownNewsSource, Guid>> GetId =>
            unknownNewsSource => unknownNewsSource.Id;

        internal static Expression<Func<UnknownNewsSource, UnknownNewsSource>> GetUnknownNewsSource =>
            unknownNewsSource => new UnknownNewsSource
            {
                Id = unknownNewsSource.Id,
                Uri = unknownNewsSource.Uri,
                Authenticity = unknownNewsSource.Authenticity
            };

        internal static Expression<Func<UnknownNewsSource, GetAllUnknownSources>> GetAll =>
            unknownNewsSource => new GetAllUnknownSources
            {
                Id = unknownNewsSource.Id,
                Domain = unknownNewsSource.Uri,
                RequestCount = unknownNewsSource.RequestCount,
                RequestedAt = unknownNewsSource.FirstRequestedAt.ToString("u"),
                IdentifiedAt = unknownNewsSource.IdentifiedAt.HasValue ?
                    unknownNewsSource.IdentifiedAt.Value.ToString("u") :
                    null,
                NewsSourceAuthenticity = unknownNewsSource.Authenticity
            };
    }
}
