using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CriThink.Server.Infrastructure.Repositories
{
    internal class StatisticsRepository : IStatisticsRepository
    {
        private readonly CriThinkDbContext _context;
        private readonly string _connectionString;

        public StatisticsRepository(CriThinkDbContext context)
        {
            _context = context;
            _connectionString = context.Database.GetConnectionString();
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<long> GetTotalNumberOfRatesArticlesAsync(CancellationToken cancellationToken = default)
        {
            const string query = "SELECT\n" +
                                 "count(*) as count\n" +
                                 "FROM article_answers";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync(cancellationToken);
            await reader.ReadAsync(cancellationToken);

            var result = long.Parse(reader["count"].ToString());

            return result;
        }

        public async Task<long> GetTotalNumberOfUserSearchesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            const string startingQuery = "SELECT\n" +
                                         "count(*)";

            const string endingQuery = " as count\n" +
                                        "FROM user_searches";

            var query = new StringBuilder(startingQuery);

            var userIdPar = new NpgsqlParameter("id", userId);
            var userFilter = $"filter (where user_id = '{userIdPar.Value}')";
            query.Append(userFilter);

            query.Append(endingQuery);

            var finalQuery = query.ToString();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(finalQuery, connection);

            var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            await reader.ReadAsync(cancellationToken);

            var result = long.Parse(reader["count"].ToString());

            return result;
        }

        public async Task<long> GetTotalNumberOfSearchesAsync(CancellationToken cancellationToken = default)
        {
            const string query = "SELECT\n" +
                                 "count(*) as count\n" +
                                 "FROM user_searches";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            await reader.ReadAsync(cancellationToken);

            var result = long.Parse(reader["count"].ToString());

            return result;
        }

        public async Task<long> GetTotalNumberOfUserAsync(CancellationToken cancellationToken = default)
        {
            const string query = "SELECT\n" +
                                 "count(*) filter (where deletion_scheduled_on is null) as count\n" +
                                 "FROM users";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(query, connection);

            var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            await reader.ReadAsync(cancellationToken);

            var result = long.Parse(reader["count"].ToString());
            return result;
        }
    }
}
