using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Handlers
{
    internal class DeleteUserLogicallyHandler : IRequestHandler<DeleteUserLogicallyCommand, User>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<DeleteUserLogicallyHandler> _logger;

        public DeleteUserLogicallyHandler(CriThinkDbContext dbContext, ILogger<DeleteUserLogicallyHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public async Task<User> Handle(DeleteUserLogicallyCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var user = _dbContext.Users
                    .FirstOrDefault(u => u.Id == request.UserId);

                user.Delete();

                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting logically the user");
                throw;
            }
        }
    }
}
