using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace CriThink.Server.Infrastructure.Handlers
{
    // ReSharper disable once UnusedMember.Global
    internal class CreateDebunkingNewsHandler : IRequestHandler<CreateDebunkingNewsCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateDebunkingNewsCommand> _logger;

        public CreateDebunkingNewsHandler(CriThinkDbContext dbContext, ILogger<CreateDebunkingNewsCommand> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateDebunkingNewsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                foreach (var news in request.DebunkingNewsCollection)
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
                    var title = new NpgsqlParameter("title", news.Title);
                    var publishingDate = new NpgsqlParameter("publishing_date", news.PublishingDate);
                    var link = new NpgsqlParameter("link", news.Link);
                    var newsCaption = new NpgsqlParameter("news_caption", news.NewsCaption);
                    var publisherName = new NpgsqlParameter("publisher_id", news.Publisher.Id);
                    var keywords = new NpgsqlParameter("keywords", news.Keywords);
                    var imageLink = new NpgsqlParameter("image_link", news.ImageLink);

                    await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery,
                        id,
                        title,
                        publishingDate,
                        link,
                        newsCaption,
                        publisherName,
                        keywords,
                        imageLink).ConfigureAwait(false);
                }

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding debunked news collection", request);
                throw;
            }

            return Unit.Value;
        }
    }
}
