using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class UnknownNewsSourceProjection
    {
        internal static Expression<Func<UnknownNewsSource, Guid>> GetId =>
            unknownNewsSource => unknownNewsSource.Id;
    }
}
