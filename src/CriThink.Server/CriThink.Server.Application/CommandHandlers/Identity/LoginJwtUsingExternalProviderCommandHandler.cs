using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.Delegates;
using CriThink.Server.Domain.DomainServices;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Interfaces;
using CriThink.Server.Domain.Models.DTOs;
using CriThink.Server.Domain.Models.LoginProviders;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class LoginJwtUsingExternalProviderCommandHandler : BaseUserCommandHandler<LoginJwtUsingExternalProviderCommand, UserLoginResponse>
    {
        private readonly ExternalLoginProviderResolver _externalLoginProviderResolver;
        private readonly IFileService _fileService;
        private readonly ILogger<LoginJwtUsingExternalProviderCommandHandler> _logger;

        public LoginJwtUsingExternalProviderCommandHandler(
            ExternalLoginProviderResolver externalLoginProviderResolver,
            IUserRepository userRepository,
            IJwtManager jwtManager,
            IHttpContextAccessor httpContext,
            IFileService fileService,
            ILogger<LoginJwtUsingExternalProviderCommandHandler> logger)
                : base(userRepository, jwtManager, httpContext)
        {
            _externalLoginProviderResolver = externalLoginProviderResolver ??
                throw new ArgumentNullException(nameof(externalLoginProviderResolver));

            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));

            _logger = logger;
        }

        public override async Task<UserLoginResponse> Handle(
            LoginJwtUsingExternalProviderCommand request,
            CancellationToken cancellationToken)
        {
            IExternalLoginProvider socialLoginProvider = _externalLoginProviderResolver(request.SocialProvider);

            var decodedToken = Base64Helper.FromBase64(request.UserToken);

            var userAccessInfo = await socialLoginProvider.GetUserAccessInfo(decodedToken);

            var provider = request.SocialProvider.ToString().ToUpperInvariant();

            var user = await UserRepository.FindUserByLoginAsync(provider, userAccessInfo.UserId);
            if (user is null)
            {
                user = await CreateExternalProviderLoginUserAsync(userAccessInfo, provider);
            }

            var refreshToken = JwtManager.GenerateRefreshToken();

            await AddRefreshTokenToUserAsync(refreshToken, user);

            var jwtToken = await JwtManager.GenerateUserJwtTokenAsync(user).ConfigureAwait(false);

            return new UserLoginResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        private async Task<User> CreateExternalProviderLoginUserAsync(
            ExternalProviderUserInfo userAccessInfo,
            string providerName)
        {
            var user = User.Create(
                userAccessInfo.Username,
                userAccessInfo.Email);

            var userCreationResult = await UserRepository.CreateUserAsync(user);
            if (!userCreationResult.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(userCreationResult, "CreateNewUser");
                _logger?.LogError(ex, "Error creating a new user {user} {Errors}", user, userCreationResult.Errors);
                throw ex;
            }

            user.ConfirmEmail();

            await UserRepository.AddUserToRoleAsync(user, RoleNames.FreeUser);

            await AddClaimsToUserAsync(user);

            if (userAccessInfo.ProfileAvatarBytes != null)
            {
                try
                {
                    await user.UpdateUserProfileAvatarAsync(
                        _fileService,
                        userAccessInfo.ProfileAvatarBytes);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error updating a new social user", user);
                    await user.DeleteUserUserProfileAvatarAsync(_fileService);
                    throw;
                }
            }

            var userLoginInfo = new UserLoginInfo(providerName, userAccessInfo.UserId, providerName);

            var loginAssociated = await UserRepository.AddUserLoginAsync(user, userLoginInfo)
                .ConfigureAwait(false);

            if (!loginAssociated.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(loginAssociated);
                _logger?.LogError(ex, "Error associating external provider to user {providerName} {user}", providerName, user);
                throw ex;
            }

            await UserRepository.SignInAsync(user, false).ConfigureAwait(false);

            return await UserRepository.FindUserByLoginAsync(providerName, userAccessInfo.UserId).ConfigureAwait(false);
        }

        private async Task AddClaimsToUserAsync(User user)
        {
            var claimsResult = await UserRepository.AddClaimsToUserAsync(user);
            if (!claimsResult.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(claimsResult);
                _logger?.LogError(ex, "Error adding user to role", user);
                throw ex;
            }
        }
    }
}
