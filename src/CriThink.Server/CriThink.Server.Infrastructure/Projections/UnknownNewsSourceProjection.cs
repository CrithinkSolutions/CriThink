using System;
using System.Linq.Expressions;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownNewsSourceProjection
    {
        internal static Expression<Func<UnknownNewsSource, UnknownNewsSource>> GetUnknownNewsSource =>
            unknownNewsSource => unknownNewsSource;

        internal static Expression<Func<UnknownNewsSource, GetAllUnknownSourcesQueryResult>> GetAll =>
            unknownNewsSource => new GetAllUnknownSourcesQueryResult
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
