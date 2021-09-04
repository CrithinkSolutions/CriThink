using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods.DbSets;
using CriThink.Server.Infrastructure.Projections;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class DebunkingNewsRepository : IDebunkingNewsRepository
    {
        private readonly CriThinkDbContext _dbContext;

        public DebunkingNewsRepository(
            CriThinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task<IList<GetAllDebunkingNewsQueryResult>> GetAllDebunkingNewsAsync(
            int pageSize,
            int pageIndex,
            string languageFilter = null)
        {
            var debunkingNewsCollection = await _dbContext.DebunkingNews.GetAllDebunkingNewsAsync(
                pageSize,
                pageIndex,
                DebunkingNewsProjection.GetAll,
                languageFilter);

            return debunkingNewsCollection;
        }

        public async Task<DebunkingNews> GetDebunkingNewsByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var debunkingNews = await _dbContext.DebunkingNews.GetDebunkingNewsAsync(
                id,
                cancellationToken);

            return debunkingNews;
        }

        public async Task AddDebunkingNewsAsync(
            DebunkingNews debunkingNews,
            CancellationToken cancellationToken = default)
        {
            var sqlQuery = "INSERT INTO debunking_news\n" +
                "(id, title, publishing_date, link, news_caption, publisher_id, keywords, image_link)\n" +
                "VALUES\n" +
                "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})\n" +
                "ON CONFLICT (link)\n" +
                "DO UPDATE\n" +
                "SET\n" +
                "title = EXCLUDED.title,\n" +
                "news_caption = EXCLUDED.news_caption,\n" +
                "image_link = EXCLUDED.image_link,\n" +
                "keywords = EXCLUDED.keywords;";

            var id = new NpgsqlParameter("id", Guid.NewGuid());
            var title = new NpgsqlParameter("title", debunkingNews.Title);
            var publishingDate = new NpgsqlParameter("publishing_date", debunkingNews.PublishingDate);
            var link = new NpgsqlParameter("link", debunkingNews.Link);
            var newsCaption = new NpgsqlParameter("news_caption", debunkingNews.NewsCaption);
            var publisherName = new NpgsqlParameter("publisher_id", debunkingNews.Publisher.Id);
            var keywords = new NpgsqlParameter("keywords", debunkingNews.Keywords);
            var imageLink = new NpgsqlParameter("image_link", debunkingNews.ImageLink);

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery,
                id,
                title,
                publishingDate,
                link,
                newsCaption,
                publisherName,
                keywords,
                imageLink);
        }

        public async Task RemoveDebunkingNewsByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var debunkingNews = await _dbContext.DebunkingNews.FindAsync(new object[] { id });
            _dbContext.DebunkingNews.Remove(debunkingNews);
        }

        public async Task UpdateDebunkingNewsAsync(
            Guid id,
            string title,
            string caption,
            string link,
            IList<string> keywords,
            string imageLink,
            CancellationToken cancellationToken = default)
        {
            var debunkingNews = await _dbContext.DebunkingNews.FindAsync(new object[] { id });

            if (!string.IsNullOrWhiteSpace(caption))
                debunkingNews.UpdateNewsCaption(caption);

            if (!string.IsNullOrWhiteSpace(title))
                debunkingNews.UpdateTitle(title);

            if (!string.IsNullOrWhiteSpace(link))
                debunkingNews.UpdateLink(link);

            if (keywords != null && keywords.Any())
                debunkingNews.SetKeywords(keywords);

            if (!string.IsNullOrWhiteSpace(imageLink))
                debunkingNews.UpdateImageLink(imageLink);

            _dbContext.DebunkingNews.Update(debunkingNews);
        }

        public async Task<IList<GetAllDebunkingNewsByKeywordsQueryResult>> GetAllDebunkingNewsByKeywordsAsync(
            IEnumerable<string> keywords,
            CancellationToken cancellationToken = default)
        {
            const string query = "SELECT dn.id, dn.title, dn.link, dn.news_caption, dn.publishing_date, dn.image_link, p.name as publisher_name\n" +
                                 "FROM debunking_news dn\n" +
                                 "JOIN debunking_news_publishers p ON p.id = dn.publisher_id\n" +
                                 "ORDER BY dnews_get_related_news(dn.keywords, :keywords) DESC\n" +
                                 "LIMIT 5;";

            var result = new List<GetAllDebunkingNewsByKeywordsQueryResult>();

            await using var connection = new NpgsqlConnection(_dbContext.Database.GetConnectionString());
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(query, connection);

            var keywordsPar = new NpgsqlParameter<string[]>("keywords", NpgsqlDbType.Array | NpgsqlDbType.Char)
            {
                Value = keywords.ToArray()
            };

            command.Parameters.Add(keywordsPar);

            var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var response = new GetAllDebunkingNewsByKeywordsQueryResult
                {
                    Id = Guid.Parse(reader["id"].ToString()),
                    Title = reader["title"].ToString(),
                    NewsLink = reader["link"].ToString(),
                    NewsCaption = reader["news_caption"].ToString(),
                    PublishingDate = reader["publishing_date"].ToString(),
                    NewsImageLink = reader["image_link"].ToString(),
                    PublisherName = reader["publisher_name"].ToString(),
                };

                result.Add(response);
            }

            return result;
        }
    }
}
