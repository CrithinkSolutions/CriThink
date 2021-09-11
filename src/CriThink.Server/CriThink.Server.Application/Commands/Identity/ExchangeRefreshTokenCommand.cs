using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Exchange current user access and refresh tokens
    /// </summary>
    public class ExchangeRefreshTokenCommand : IRequest<UserRefreshTokenResponse>
    {
        public ExchangeRefreshTokenCommand(
            string accessToken,
            string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; }

        public string RefreshToken { get; }
    }
}
