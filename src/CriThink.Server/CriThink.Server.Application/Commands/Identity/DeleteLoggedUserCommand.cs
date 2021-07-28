using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Logically delete the current user
    /// </summary>
    public class DeleteLoggedUserCommand : IRequest<UserSoftDeletionResponse>
    {
    }
}
