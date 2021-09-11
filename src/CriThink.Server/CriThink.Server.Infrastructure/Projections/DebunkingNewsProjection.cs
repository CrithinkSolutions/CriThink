using System;
using System.Linq.Expressions;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.QueryResults;

namespace CriThink.Server.Infrastructure.Projections
{
    internal static class DebunkingNewsProjection
    {
        internal static Expression<Func<DebunkingNews, GetAllDebunkingNewsQueryResult>> GetAll =>
            debunkingNews => new GetAllDebunkingNewsQueryResult
            {
                Id = debunkingNews.Id,
                Publisher = debunkingNews.Publisher.Name,
                PublisherCountry = debunkingNews.Publisher.Country.Name,
                PublisherLanguage = debunkingNews.Publisher.Language.Name,
                Title = debunkingNews.Title,
                NewsImageLink = debunkingNews.ImageLink,
                NewsCaption = debunkingNews.NewsCaption,
                NewsLink = debunkingNews.Link,
            };
    }
}
