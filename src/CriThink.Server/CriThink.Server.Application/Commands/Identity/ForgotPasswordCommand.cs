using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Generates a temporary token for the user, in order to reset its
    /// </summary>
    public class ForgotPasswordCommand : IRequest
    {
        public ForgotPasswordCommand(string email, string userName)
        {
            Email = email;
            Username = userName;
        }

        public string Email { get; }

        public string Username { get; }
    }
}
