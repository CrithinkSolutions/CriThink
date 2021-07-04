using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Queries;
using CriThink.Server.Core.Responses;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class GetStatisticsSearchesCountingHandler : IRequestHandler<GetStatisticsSearchesCountingQuery, GetStatisticsSearchesCountingQueryResponse>
    {
        private readonly string _connectionString;
        private readonly ILogger<GetStatisticsSearchesCountingHandler> _logger;

        public GetStatisticsSearchesCountingHandler(IConfiguration configuration, ILogger<GetStatisticsSearchesCountingHandler> logger)
        {
            _connectionString = configuration?.GetConnectionString("CriThinkDbPgSqlConnection") ??
                                throw new ArgumentNullException(nameof(configuration));

            _logger = logger;
        }

        public async Task<GetStatisticsSearchesCountingQueryResponse> Handle(GetStatisticsSearchesCountingQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                const string startingQuery = "SELECT\n" +
                                        "count(*)";

                const string endingQuery = " as count\n" +
                                           "FROM user_searches\n";

                var query = new StringBuilder(startingQuery);

                if (request.UserId is not null)
                {
                    var userId = new NpgsqlParameter("id", request.UserId);
                    var userFilter = $"filter (where user_id = '{userId.Value}')";
                    query.Append(userFilter);
                }

                query.Append(endingQuery);

                var finalQuery = query.ToString();

                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                await using var command = new NpgsqlCommand(finalQuery, connection);

                var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                await reader.ReadAsync(cancellationToken);

                var result = long.Parse(reader["count"].ToString());
                return new GetStatisticsSearchesCountingQueryResponse(result);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting number of searches");
                throw;
            }
        }
    }
}
