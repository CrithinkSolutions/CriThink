using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Login using an external provider
    /// </summary>
    public class LoginJwtUsingExternalProviderCommand : IRequest<UserLoginResponse>
    {
        public LoginJwtUsingExternalProviderCommand(ExternalLoginProvider socialProvider, string userToken)
        {
            SocialProvider = socialProvider;
            UserToken = userToken;
        }

        public ExternalLoginProvider SocialProvider { get; }

        public string UserToken { get; }
    }
}
