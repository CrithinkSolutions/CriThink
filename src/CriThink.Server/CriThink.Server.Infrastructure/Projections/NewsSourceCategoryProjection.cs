using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class NewsSourceCategoryProjection
    {
        internal static Expression<Func<NewsSourceCategory, string>> GetDescription =>
            category => category.Description;
    }
}
