using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateUserProfileAvatarCommandHandler : IRequestHandler<UpdateUserProfileAvatarCommand>
    {
        private readonly IUserAvatarService _userAvatarService;
        private readonly ILogger<UpdateUserProfileAvatarCommandHandler> _logger;

        public UpdateUserProfileAvatarCommandHandler(
            IUserAvatarService userAvatarService,
            ILogger<UpdateUserProfileAvatarCommandHandler> logger)
        {
            _userAvatarService = userAvatarService ??
                throw new ArgumentNullException(nameof(userAvatarService));

            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserProfileAvatarCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Updating UserProfile Avatar");

            await _userAvatarService.UpdateUserProfileAvatarAsync(
                request.UserId,
                request.FormFile,
                cancellationToken);

            return Unit.Value;
        }
    }
}
