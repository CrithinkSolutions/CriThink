using System;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Verify the user email through the email link
    /// </summary>
    public class VerifyAccountEmailCommand : IRequest<VerifyUserEmailResponse>
    {
        public VerifyAccountEmailCommand(
            Guid userId,
            string code)
        {
            UserId = userId;
            Code = code;
        }

        public Guid UserId { get; }

        public string Code { get; }
    }
}
