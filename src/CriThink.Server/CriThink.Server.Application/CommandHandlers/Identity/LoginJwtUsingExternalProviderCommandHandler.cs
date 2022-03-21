using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Domain.DomainServices;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.Interfaces;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using CriThink.Server.Infrastructure.Delegates;
using CriThink.Server.Infrastructure.ExtensionMethods;
using CriThink.Server.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class LoginJwtUsingExternalProviderCommandHandler
        : BaseUserCommandHandler<LoginJwtUsingExternalProviderCommand, UserLoginResponse>
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
            var auth = await HttpContext.AuthenticateAsync(request.SocialProvider.ToString());

            if (!auth.Succeeded
                    || auth?.Principal == null
                    || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                    || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                return new UserLoginResponse();
            }

            ExternalLoginInfo info = await UserRepository.GetExternalLoginInfoAsync();
            var userEmail = info.Principal.GetEmail();

            var crithinkUser = await UserRepository.FindUserAsync(userEmail, cancellationToken);
            if (crithinkUser is null)
            {
                crithinkUser = await CreateExternalProviderLoginUserAsync(info, request.SocialProvider);
            }

            var refreshToken = JwtManager.GenerateRefreshToken();

            await AddRefreshTokenToUserAsync(refreshToken, crithinkUser);

            var jwtToken = await JwtManager.GenerateUserJwtTokenAsync(crithinkUser);

            await UserRepository.UpdateUserAsync(crithinkUser);

            return new UserLoginResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        private async Task<User> CreateExternalProviderLoginUserAsync(
            ExternalLoginInfo loginInfo,
            ExternalLoginProvider socialProvider)
        {
            var user = User.Create(
                username: loginInfo.Principal.GetFullName(),
                email: loginInfo.Principal.GetEmail());

            var userCreationResult = await UserRepository.CreateUserAsync(user);
            if (!userCreationResult.Succeeded)
            {
                var ex = new CriThinkIdentityOperationException(userCreationResult, "CreateNewUser");

                _logger?.LogError(
                    ex,
                    "Error creating a new user: {0} {1}, {2}",
                    user.Id,
                    user.Email,
                    userCreationResult.Errors);

                throw ex;
            }

            user.ConfirmEmail();
            user.UpdateGivenName(loginInfo.Principal.GetGivenName());
            user.UpdateFamilyName(loginInfo.Principal.GetFamilyName());

            try
            {
                IExternalLoginProvider socialLoginProvider = _externalLoginProviderResolver(socialProvider);
                var userAccessInfo = await socialLoginProvider.GetUserAccessInfoAsync(loginInfo);

                user.UpdateCountry(userAccessInfo.Country);
                user.UpdateDateOfBirth(userAccessInfo.Birthday);
                user.UpdateGender(userAccessInfo.Gender);

                await user.UpdateUserProfileAvatarAsync(
                    fileService: _fileService,
                    bytes: userAccessInfo.ProfileAvatarBytes);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(
                    ex,
                    "Can't get external user info with schema {schema}",
                    socialProvider);
            }

            await UserRepository.UpdateUserAsync(user);

            await UserRepository.AddUserToRoleAsync(user, RoleNames.FreeUser);

            await AddClaimsToUserAsync(user);

            return user;
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
