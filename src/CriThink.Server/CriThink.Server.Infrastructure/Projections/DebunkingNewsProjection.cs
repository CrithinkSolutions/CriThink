using System;
using System.Linq.Expressions;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Responses;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class DebunkingNewsProjection
    {
        internal static Expression<Func<DebunkingNews, GetAllDebunkingNewsQueryResponse>> GetAll =>
            debunkingNews => new GetAllDebunkingNewsQueryResponse
            {
                Id = debunkingNews.Id,
                Publisher = debunkingNews.PublisherName,
                Title = debunkingNews.Title
            };
    }
}
