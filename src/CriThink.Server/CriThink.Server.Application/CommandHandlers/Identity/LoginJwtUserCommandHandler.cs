using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class LoginJwtUserCommandHandler : BaseUserCommandHandler<LoginJwtUserCommand, UserLoginResponse>
    {
        private readonly ILogger<LoginJwtUserCommandHandler> _logger;

        public LoginJwtUserCommandHandler(
            IUserRepository userRepository,
            IJwtManager jwtManager,
            IHttpContextAccessor httpContext,
            ILogger<LoginJwtUserCommandHandler> logger)
            : base(userRepository, jwtManager, httpContext)
        {
            _logger = logger;
        }

        public override async Task<UserLoginResponse> Handle(LoginJwtUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("Login User");

            var user = await UserRepository.FindUserAsync(request.Email ?? request.Username, cancellationToken);

            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"Email: '{request.Email}' - Username: '{request.Username}'");

            if (user.IsDeleted || !user.EmailConfirmed)
                throw new InvalidOperationException("The user is not enabled");

            var verificationResult = UserRepository.VerifyUserPassword(user, request.Password);

            await ProcessPasswordVerificationResultAsync(user, verificationResult);

            var refreshToken = JwtManager.GenerateToken();

            await AddRefreshTokenToUserAsync(refreshToken, user);

            var jwtToken = await JwtManager.GenerateUserJwtTokenAsync(user);

            return new UserLoginResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        private async Task ProcessPasswordVerificationResultAsync(User user, PasswordVerificationResult verificationResult)
        {
            switch (verificationResult)
            {
                case PasswordVerificationResult.Failed:
                    throw new ResourceNotFoundException("Password is not correct");
                case PasswordVerificationResult.SuccessRehashNeeded:
                    await UpdateUserPasswordHashAsync(user).ConfigureAwait(false);
                    break;
                case PasswordVerificationResult.Success:
                    return;
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task UpdateUserPasswordHashAsync(User user)
        {
            var result = await UserRepository.HasUserPasswordAsync(user);
            if (!result)
            {
                var ex = new InvalidOperationException("Error hashing again user password");
                _logger?.LogError(ex, "Rehash needed but failed", user);
            }
        }
    }
}
