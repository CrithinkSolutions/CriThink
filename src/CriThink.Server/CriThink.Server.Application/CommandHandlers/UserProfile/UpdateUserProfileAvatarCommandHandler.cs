using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.DomainServices;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class UpdateUserProfileAvatarCommandHandler : IRequestHandler<UpdateUserProfileAvatarCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly ILogger<UpdateUserProfileAvatarCommandHandler> _logger;

        public UpdateUserProfileAvatarCommandHandler(
            IUserRepository userRepository,
            IFileService fileService,
            ILogger<UpdateUserProfileAvatarCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));

            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserProfileAvatarCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(UpdateUserProfileAvatarCommandHandler));

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user is null)
                throw new ResourceNotFoundException("User not found", request.UserId);

            await user.UpdateUserProfileAvatarAsync(
                _fileService,
                request.FormFile);

            await _userRepository.UpdateUserAsync(user);

            _logger?.LogInformation($"{nameof(UpdateUserProfileAvatarCommandHandler)}: done");

            return Unit.Value;
        }
    }
}
