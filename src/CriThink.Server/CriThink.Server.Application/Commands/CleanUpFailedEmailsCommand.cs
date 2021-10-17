using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Try to send failed emails again
    /// </summary>
    public class CleanUpFailedEmailsCommand : IRequest
    {
    }
}
