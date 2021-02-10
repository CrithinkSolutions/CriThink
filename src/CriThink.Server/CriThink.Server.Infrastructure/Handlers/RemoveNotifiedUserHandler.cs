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
    public class RemoveNotifiedUserHandler : IRequestHandler<RemoveNotifiedUserCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<RemoveNotifiedUserHandler> _logger;

        public RemoveNotifiedUserHandler(CriThinkDbContext dbContext, ILogger<RemoveNotifiedUserHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveNotifiedUserCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var itemToRemove = await _dbContext.UnknownNewsSourceNotificationRequests
                                                   .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                                                   .ConfigureAwait(false);

                _dbContext.UnknownNewsSourceNotificationRequests.Remove(itemToRemove);

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
