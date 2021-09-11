using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class DeleteLoggedUserCommandHandler : IRequestHandler<DeleteUserCommand, UserSoftDeletionResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteLoggedUserCommandHandler> _logger;

        public DeleteLoggedUserCommandHandler(
            IUserRepository userRepository,
            ILogger<DeleteLoggedUserCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _logger = logger;
        }

        public async Task<UserSoftDeletionResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("DeleteLoggedUser");

            var user = await _userRepository.DeleteUserByIdAsync(request.Id);

            _logger?.LogInformation("DeleteLoggedUser: done");

            return new UserSoftDeletionResponse()
            {
                DeletionScheduledOn = user.DeletionScheduledOn.Value
            };
        }
    }
}
