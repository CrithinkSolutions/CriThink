using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Responses;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class DeleteUserScheduledDeletionHandler : IRequestHandler<DeleteUserScheduledDeletionCommand, DeleteUserScheduledDeletionCommandResponse>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<DeleteUserScheduledDeletionHandler> _logger;

        public DeleteUserScheduledDeletionHandler(CriThinkDbContext dbContext, ILogger<DeleteUserScheduledDeletionHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<DeleteUserScheduledDeletionCommandResponse> Handle(DeleteUserScheduledDeletionCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                const string sqlCommand = "DELETE FROM users\n" +
                                          "WHERE deletion_scheduled_on < now() AT TIME ZONE 'UTC'\n" +
                                          "RETURNING *";

                var deletedUsers = await _dbContext.Users
                    .FromSqlRaw(sqlCommand)
                    .AsNoTracking()
                    .ToListAsync();

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new DeleteUserScheduledDeletionCommandResponse(deletedUsers);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting users with pending deletion");
                throw;
            }
        }
    }
}
