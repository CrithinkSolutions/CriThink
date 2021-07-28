using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        public DeleteUserCommandHandler(
            CriThinkDbContext dbContext,
            ILogger<DeleteUserCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("DeleteUser");

            var userId = request.Id;

            var user = _dbContext.Users
                    .FirstOrDefault(u => u.Id == userId);

            user.Delete();

            await _dbContext.SaveChangesAsync();

            _logger?.LogInformation("DeleteUser: done");

            return Unit.Value;
        }
    }
}
