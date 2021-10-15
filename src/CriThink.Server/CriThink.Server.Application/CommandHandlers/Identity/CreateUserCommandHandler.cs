using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.DomainServices;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Providers.EmailSender.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserSignUpResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly IEmailSenderService _emailSender;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IFileService fileService,
            IEmailSenderService emailSender,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));

            _emailSender = emailSender ??
                throw new ArgumentNullException(nameof(emailSender));

            _logger = logger;
        }

        public async Task<UserSignUpResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation(nameof(CreateUserCommandHandler));

            var user = User.Create(
                request.Username,
                request.Email);

            await CreateUserAsync(user, request.Password);

            await _userRepository.AddUserToRoleAsync(user, RoleNames.FreeUser);

            await AddClaimsToUserAsync(user);

            if (request.FormFile is not null)
            {
                _logger?.LogInformation("Add User: custom avatar");
                await user.UpdateUserProfileAvatarAsync(_fileService, request.FormFile);
            }

            try
            {
                await _userRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating a new user", user);
                await user.DeleteUserUserProfileAvatarAsync(_fileService);
                throw;
            }

            var confirmationCode = await _userRepository.GetEmailConfirmationTokenAsync(user);

            await _emailSender.SendAccountConfirmationEmailAsync(
                user.Email,
                user.Id.ToString(),
                confirmationCode,
                user.UserName);

            _logger?.LogInformation($"{nameof(CreateUserCommandHandler)}: done");

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
                var ex = new CriThinkIdentityOperationException(userCreationResult, "CreateNewUser");
                _logger?.LogError(ex, "Error creating a new user", user, userCreationResult.Errors);
                throw ex;
            }
        }

        private async Task AddClaimsToUserAsync(User user)
        {
            var claimsResult = await _userRepository.AddClaimsToUserAsync(user);
            if (!claimsResult.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(claimsResult);
                _logger?.LogError(ex, "Error adding user to role", user);
                throw ex;
            }
        }
    }
}
