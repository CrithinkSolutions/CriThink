using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class DemoNewsProjection
    {
        /// <summary>
        /// Get all the items and converts them to a list of <see cref="DemoNews"/>
        /// </summary>
        internal static Expression<Func<DemoNews, DemoNews>> GetAll =>
            demoNews => new DemoNews
            {
                Id = demoNews.Id,
                Link = demoNews.Link,
                Title = demoNews.Title
            };
    }
}
