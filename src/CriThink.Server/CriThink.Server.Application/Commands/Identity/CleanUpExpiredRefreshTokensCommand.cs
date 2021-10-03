using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Clean up user with expired
    /// refresh tokens
    /// </summary>
    public class CleanUpExpiredRefreshTokensCommand : IRequest
    {
    }
}
