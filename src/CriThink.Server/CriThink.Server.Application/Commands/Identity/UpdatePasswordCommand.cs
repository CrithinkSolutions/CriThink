using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Allow user to change its personal password
    /// </summary>
    public class UpdatePasswordCommand : IRequest
    {
        public UpdatePasswordCommand(string currentPassword, string newPassword)
        {
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }

        public string CurrentPassword { get; }

        public string NewPassword { get; }
    }
}
