using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Services;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserSignUpResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAvatarService _userAvatarService;
        private readonly IEmailSenderService _emailSender;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IUserAvatarService userAvatarService,
            IEmailSenderService emailSender,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _userAvatarService = userAvatarService ??
                throw new ArgumentNullException(nameof(userAvatarService));

            _emailSender = emailSender ??
                throw new ArgumentNullException(nameof(emailSender));

            _logger = logger;
        }

        public async Task<UserSignUpResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Add User");

            var user = User.Create(
                request.Username,
                request.Email);

            await CreateUserAsync(user, request.Password);

            await AddClaimsToUserAsync(user);

            if (request.FormFile is not null)
            {
                _logger?.LogInformation("Add User: customa avatar");

                await _userAvatarService.UpdateUserProfileAvatarAsync(user.Id, request.FormFile, cancellationToken);
            }

            var confirmationCode = await _userRepository.GetEmailConfirmationTokenAsync(user);

            await _emailSender.SendAccountConfirmationEmailAsync(user.Email, user.Id.ToString(), confirmationCode, user.UserName);

            _logger?.LogInformation("Add User: done");

            return new UserSignUpResponse
            {
                UserId = user.Id.ToString(),
                UserEmail = user.Email,
            };
        }

        private async Task CreateUserAsync(User user, string password)
        {
            var userCreationResult = await _userRepository.CreateUserAsync(user, password);
            if (!userCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(userCreationResult, "CreateNewUser");
                _logger?.LogError(ex, "Error creating a new user", user, userCreationResult.Errors);
                throw ex;
            }
        }

        private async Task AddClaimsToUserAsync(User user)
        {
            var claimsResult = await _userRepository.AddClaimsToUserAsync(user);
            if (!claimsResult.Succeeded)
            {
                var ex = new IdentityOperationException(claimsResult);
                _logger?.LogError(ex, "Error adding user to role", user);
                throw ex;
            }
        }
    }
}
