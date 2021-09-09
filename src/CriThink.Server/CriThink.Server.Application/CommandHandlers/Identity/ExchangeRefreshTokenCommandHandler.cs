using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.CommandHandlers
{
    internal class ExchangeRefreshTokenCommandHandler : BaseUserCommandHandler<ExchangeRefreshTokenCommand, UserRefreshTokenResponse>
    {
        private readonly ILogger<ExchangeRefreshTokenCommandHandler> _logger;

        public ExchangeRefreshTokenCommandHandler(
            IUserRepository userRepository,
            IJwtManager jwtManager,
            IHttpContextAccessor httpContext,
            ILogger<ExchangeRefreshTokenCommandHandler> logger)
            : base(userRepository, jwtManager, httpContext)
        {
            _logger = logger;
        }

        public override async Task<UserRefreshTokenResponse> Handle(ExchangeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("ExchangeRefreshToken");

            var oldRefreshToken = request.RefreshToken;

            var userIdClaim = JwtManager.GetPrincipalFromToken(request.AccessToken)
                ?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value ?? throw new InvalidOperationException("The given JWT token is not valid");

            var user = await UserRepository.FindUserAsync(userIdClaim);

            var refreshToken = await ReplaceRefreshToken(user, oldRefreshToken);

            var jwtToken = await JwtManager.GenerateUserJwtTokenAsync(user);

            _logger?.LogInformation("ExchangeRefreshToken: done");

            return new UserRefreshTokenResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        private async Task<string> ReplaceRefreshToken(User user, string oldRefreshToken)
        {
            if (user?.HasValidRefreshToken(oldRefreshToken) != true)
                throw new CriThinkRefreshTokenExpiredException();

            user.RemoveRefreshToken(oldRefreshToken);

            var refreshToken = JwtManager.GenerateToken();

            var lifetimeFromNow = JwtManager.GetDefaultRefreshTokenLifetime();
            user.AddRefreshToken(refreshToken, HttpContext?.Connection.RemoteIpAddress?.ToString(), lifetimeFromNow);

            await UserRepository.UpdateUserAsync(user);

            return refreshToken;
        }
    }
}
