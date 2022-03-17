using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class LoginJwtUsingSocialProviderCommand : IRequest<UserLoginResponse>
    {
        public LoginJwtUsingSocialProviderCommand(string scheme)
        {
            Scheme = scheme;
        }

        public string Scheme { get; }
    }
}
