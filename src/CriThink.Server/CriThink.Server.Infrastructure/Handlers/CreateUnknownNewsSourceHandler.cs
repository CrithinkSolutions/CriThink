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
    public class CreateUnknownNewsSourceHandler : IRequestHandler<CreateUnknownNewsSourceCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<CreateUnknownNewsSourceHandler> _logger;

        public CreateUnknownNewsSourceHandler(CriThinkDbContext dbContext, ILogger<CreateUnknownNewsSourceHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateUnknownNewsSourceCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var sqlQuery = "INSERT INTO unknown_news_sources\n" +
                           "(id, uri, first_requested_at, request_count, authenticity)\n" +
                           "VALUES\n" +
                           "({0}, {1}, {2}, {3}, {4})\n" +
                           "ON CONFLICT (uri)\n" +
                           "DO UPDATE\n" +
                           "SET\n" +
                           "request_count = (unknown_news_sources.request_count + 1);";

                var id = new NpgsqlParameter("id", Guid.NewGuid());
                var uri = new NpgsqlParameter("uri", request.Uri.ToString());
                var firstRequestedAt = new NpgsqlParameter("first_requested_at", DateTime.Now);
                var requestCount = new NpgsqlParameter("request_count", 1);
                var authenticity = new NpgsqlParameter("authenticity", NewsSourceAuthenticity.Unknown.ToString());

                await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery,
                        id,
                        uri,
                        firstRequestedAt,
                        requestCount,
                        authenticity)
                    .ConfigureAwait(false);
                
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}