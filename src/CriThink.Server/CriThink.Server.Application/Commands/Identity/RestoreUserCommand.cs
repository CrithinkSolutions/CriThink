using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Restore a previously logically deleted user
    /// </summary>
    public class RestoreUserCommand : IRequest
    {
        public RestoreUserCommand(string email, string userName)
        {
            Email = email;
            Username = userName;
        }

        public string Email { get; }

        public string Username { get; }
    }
}
