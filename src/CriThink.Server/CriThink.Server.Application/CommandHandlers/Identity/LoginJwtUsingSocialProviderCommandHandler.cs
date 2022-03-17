using System;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Commands;
using MediatR;

namespace CriThink.Server.Application.CommandHandlers.Identity
{
    internal class LoginJwtUsingSocialProviderCommandHandler
        : IRequestHandler<LoginJwtUsingSocialProviderCommand, UserLoginResponse>
    {
        public LoginJwtUsingSocialProviderCommandHandler()
        {

        }

        public Task<UserLoginResponse> Handle(
            LoginJwtUsingSocialProviderCommand request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
