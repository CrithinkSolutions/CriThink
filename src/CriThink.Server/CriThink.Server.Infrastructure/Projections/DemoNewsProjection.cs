using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Infrastructure.Projections
{
    public static class DemoNewsProjection
    {
        /// <summary>
        /// Get all the items and converts them to a list of <see cref="GetAllDemoNewsQueryResponse"/>
        /// </summary>
        public static Expression<Func<DemoNews, GetAllDemoNewsQueryResponse>> GetAll =>
            demoNews => new GetAllDemoNewsQueryResponse(demoNews.Id, demoNews.Title, demoNews.Link);
    }
}
