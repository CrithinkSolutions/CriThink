using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Reset the user password using the temporary token
    /// </summary>
    public class ResetUserPasswordCommand : IRequest
    {
        public ResetUserPasswordCommand(
            string userId,
            string token,
            string newPassword)
        {
            UserId = userId;
            Token = token;
            NewPassword = newPassword;
        }

        public string UserId { get; }

        public string Token { get; }

        public string NewPassword { get; }
    }
}
