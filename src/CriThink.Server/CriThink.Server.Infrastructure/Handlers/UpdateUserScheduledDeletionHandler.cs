using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class UpdateUserScheduledDeletionHandler : IRequestHandler<UpdateUserScheduledDeletionCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<UpdateUserScheduledDeletionHandler> _logger;

        public UpdateUserScheduledDeletionHandler(CriThinkDbContext dbContext, ILogger<UpdateUserScheduledDeletionHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserScheduledDeletionCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

                user.CancelScheduledDeletion();

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating user deletion status");
                throw;
            }

            return Unit.Value;
        }
    }
}
