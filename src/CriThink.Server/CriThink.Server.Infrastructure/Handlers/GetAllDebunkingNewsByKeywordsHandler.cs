using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class GetAllDebunkingNewsByKeywordsHandler : IRequestHandler<GetAllDebunkingNewsByKeywordsQuery, IList<GetAllDebunkingNewsByKeywordsQueryResponse>>
    {
        private readonly string _connectionString;
        private readonly ILogger<GetAllDebunkingNewsByKeywordsHandler> _logger;

        public GetAllDebunkingNewsByKeywordsHandler(IConfiguration configuration, ILogger<GetAllDebunkingNewsByKeywordsHandler> logger)
        {
            _connectionString = configuration?.GetConnectionString("CriThinkDbPgSqlConnection") ??
                                throw new ArgumentNullException(nameof(configuration));

            _logger = logger;
        }

        public async Task<IList<GetAllDebunkingNewsByKeywordsQueryResponse>> Handle(GetAllDebunkingNewsByKeywordsQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var result = new List<GetAllDebunkingNewsByKeywordsQueryResponse>();

                if (!request.Keywords.Any())
                    return result;

                const string query = "SELECT dn.id, dn.title, dn.link, dn.news_caption, dn.publishing_date, dn.image_link, p.name as publisher_name\n" +
                                     "FROM debunking_news dn\n" +
                                     "JOIN debunking_news_publishers p ON p.id = dn.publisher_id\n" +
                                     "ORDER BY dnews_get_related_news(dn.keywords, :keywords) DESC\n" +
                                     "LIMIT 5;";

                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                await using var command = new NpgsqlCommand(query, connection);

                var keywords = new NpgsqlParameter<string[]>("keywords", NpgsqlDbType.Array | NpgsqlDbType.Char)
                {
                    Value = request.Keywords.ToArray()
                };

                command.Parameters.Add(keywords);

                var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

                while (await reader.ReadAsync(cancellationToken))
                {
                    var response = new GetAllDebunkingNewsByKeywordsQueryResponse
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
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting debunking news by keywords");
                throw;
            }
        }
    }
}
