using System;
using System.Linq.Expressions;
using CriThink.Common.Endpoints.DTOs.NewsSource;
using CriThink.Common.Endpoints.DTOs.UnknownNewsSource;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownNewsSourceProjection
    {
        internal static Expression<Func<UnknownNewsSource, Guid>> GetId =>
            unknownNewsSource => unknownNewsSource.Id;

        internal static Expression<Func<UnknownNewsSource, UnknownNewsSourceResponse>> GetUnknownNewsSource =>
            unknownNewsSource => new UnknownNewsSourceResponse
            {
                Id = unknownNewsSource.Id,
                Uri = unknownNewsSource.Uri,
                Classification = ToClassification(unknownNewsSource.Authenticity),
            };

        private static NewsSourceClassification ToClassification(NewsSourceAuthenticity authenticity) =>
            authenticity switch
            {
                NewsSourceAuthenticity.Conspiracist => NewsSourceClassification.Conspiracist,
                NewsSourceAuthenticity.FakeNews => NewsSourceClassification.FakeNews,
                NewsSourceAuthenticity.Reliable => NewsSourceClassification.Reliable,
                NewsSourceAuthenticity.Satirical => NewsSourceClassification.Satirical,
                NewsSourceAuthenticity.Unknown => NewsSourceClassification.Unknown,
                _ => throw new NotImplementedException(nameof(ToClassification)),
            };
    }
}
