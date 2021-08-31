using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownNewsSourceProjection
    {
        internal static Expression<Func<UnknownNewsSource, Guid>> GetId =>
            unknownNewsSource => unknownNewsSource.Id;

        internal static Expression<Func<UnknownNewsSource, UnknownNewsSource>> GetUnknownNewsSource =>
            unknownNewsSource => UnknownNewsSource.Create(
                unknownNewsSource.Id,
                unknownNewsSource.Uri,
                unknownNewsSource.Authenticity);

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
