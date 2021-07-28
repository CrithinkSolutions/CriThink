using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using CriThink.Server.Application.Commands;
using CriThink.Server.Application.Services;
using CriThink.Server.Core.Delegates;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Models.DTOs;
using CriThink.Server.Core.Models.LoginProviders;
using CriThink.Server.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class LoginJwtUsingExternalProviderCommandHandler : BaseUserCommandHandler<LoginJwtUsingExternalProviderCommand, UserLoginResponse>
    {
        private readonly ExternalLoginProviderResolver _externalLoginProviderResolver;
        private readonly IUserAvatarService _userAvatarService;
        private readonly ILogger<LoginJwtUsingExternalProviderCommandHandler> _logger;

        public LoginJwtUsingExternalProviderCommandHandler(
            ExternalLoginProviderResolver externalLoginProviderResolver,
            IUserRepository userRepository,
            IJwtManager jwtManager,
            IHttpContextAccessor httpContext,
            IUserAvatarService userAvatarService,
            ILogger<LoginJwtUsingExternalProviderCommandHandler> logger)
                : base(userRepository, jwtManager, httpContext)
        {
            _externalLoginProviderResolver = externalLoginProviderResolver ??
                throw new ArgumentNullException(nameof(externalLoginProviderResolver));

            _userAvatarService = userAvatarService ??
                throw new ArgumentNullException(nameof(userAvatarService));

            _logger = logger;
        }

        public override async Task<UserLoginResponse> Handle(LoginJwtUsingExternalProviderCommand request, CancellationToken cancellationToken)
        {
            IExternalLoginProvider socialLoginProvider = _externalLoginProviderResolver(request.SocialProvider);

            var decodedToken = Base64Helper.FromBase64(request.UserToken);

            var userAccessInfo = await socialLoginProvider.GetUserAccessInfo(decodedToken);

            var provider = request.SocialProvider.ToString().ToUpperInvariant();

            var currentUser = await UserRepository.FindUserByLoginAsync(provider, userAccessInfo.UserId);
            if (currentUser is null)
            {
                currentUser = await CreateExternalProviderLoginUserAsync(userAccessInfo, provider, cancellationToken);
            }

            var refreshToken = JwtManager.GenerateToken();
            await AddRefreshTokenToUserAsync(refreshToken, currentUser);

            var jwtToken = await JwtManager.GenerateUserJwtTokenAsync(currentUser).ConfigureAwait(false);

            return new UserLoginResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        private async Task<User> CreateExternalProviderLoginUserAsync(ExternalProviderUserInfo userAccessInfo, string providerName, CancellationToken cancellationToken)
        {
            var user = User.Create(
                userAccessInfo.Username,
                userAccessInfo.Email);

            var userCreationResult = await UserRepository.CreateUserAsync(user);
            if (!userCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(userCreationResult, "CreateNewUser");
                _logger?.LogError(ex, "Error creating a new user", user, userCreationResult.Errors);
                throw ex;
            }

            if (userAccessInfo.ProfileAvatarBytes != null)
            {
                try
                {
                    await _userAvatarService.UpdateUserProfileAvatarAsync(user.Id, userAccessInfo.ProfileAvatarBytes, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error saving user social avatar");
                }
            }

            var userLoginInfo = new UserLoginInfo(providerName, userAccessInfo.UserId, providerName);

            var userCreated = await UserRepository.CreateUserAsync(user).ConfigureAwait(false);
            if (!userCreated.Succeeded)
            {
                var ex = new IdentityOperationException(userCreated);
                _logger?.LogError(ex, "Error creating user.", providerName, user);
                throw ex;
            }

            var loginAssociated = await UserRepository.AddUserLoginAsync(user, userLoginInfo)
                .ConfigureAwait(false);

            if (!loginAssociated.Succeeded)
            {
                var ex = new IdentityOperationException(loginAssociated);
                _logger?.LogError(ex, "Error associating external provider to user.", providerName, user);
                throw ex;
            }

            await UserRepository.SignInAsync(user, false).ConfigureAwait(false);

            return await UserRepository.FindUserByLoginAsync(providerName, userAccessInfo.UserId).ConfigureAwait(false);
        }
    }
}
