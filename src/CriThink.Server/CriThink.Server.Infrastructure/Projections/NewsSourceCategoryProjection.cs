using System;
using System.Linq.Expressions;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class NewsSourceCategoryProjection
    {
        internal static Expression<Func<NewsSourceCategory, string>> GetDescription =>
            category => category.Description;
    }
}
