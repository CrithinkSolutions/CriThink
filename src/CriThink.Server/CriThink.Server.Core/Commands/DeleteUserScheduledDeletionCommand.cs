using CriThink.Server.Core.Responses;
using MediatR;

namespace CriThink.Server.Core.Commands
{
    public class DeleteUserScheduledDeletionCommand : IRequest<DeleteUserScheduledDeletionCommandResponse>
    {
    }
}
