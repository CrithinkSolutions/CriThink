using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class DeleteLoggedUserCommandHandler : IRequestHandler<DeleteLoggedUserCommand, UserSoftDeletionResponse>
    {
        private readonly HttpContext _httpContext;
        private readonly CriThinkDbContext _dbContext;
        private readonly ILogger<DeleteLoggedUserCommandHandler> _logger;

        public DeleteLoggedUserCommandHandler(
            IHttpContextAccessor httpContext,
            CriThinkDbContext dbContext,
            ILogger<DeleteLoggedUserCommandHandler> logger)
        {
            _httpContext = httpContext.HttpContext;

            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));

            _logger = logger;
        }

        public async Task<UserSoftDeletionResponse> Handle(DeleteLoggedUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("DeleteLoggedUser");

            var userId = _httpContext.User.GetId();

            var user = _dbContext.Users
                    .FirstOrDefault(u => u.Id == userId);

            user.Delete();

            await _dbContext.SaveChangesAsync();

            _logger?.LogInformation("DeleteLoggedUser: done");

            return new UserSoftDeletionResponse()
            {
                DeletionScheduledOn = user.DeletionScheduledOn.Value
            };
        }
    }
}
