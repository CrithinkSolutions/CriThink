using System;
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
    internal class GetStatisticsUserCountingHandler : IRequestHandler<GetStatisticsUserCountingQuery, GetStatisticsUserCountingQueryResponse>
    {
        private readonly string _connectionString;
        private readonly ILogger<GetStatisticsUserCountingHandler> _logger;

        public GetStatisticsUserCountingHandler(IConfiguration configuration, ILogger<GetStatisticsUserCountingHandler> logger)
        {
            _connectionString = configuration?.GetConnectionString("CriThinkDbPgSqlConnection") ??
                                throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        public async Task<GetStatisticsUserCountingQueryResponse> Handle(GetStatisticsUserCountingQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                const string query = "SELECT\n" +
                                        "count(*) filter (where deletion_scheduled_on is null) as count\n" +
                                     "FROM users\n";

                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                await using var command = new NpgsqlCommand(query, connection);

                var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                await reader.ReadAsync(cancellationToken);

                var result = long.Parse(reader["count"].ToString());
                return new GetStatisticsUserCountingQueryResponse(result);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting number of users");
                throw;
            }
        }
    }
}
