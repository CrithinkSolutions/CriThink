using System;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Allow user to change its personal password
    /// </summary>
    public class UpdatePasswordCommand : IRequest
    {
        public UpdatePasswordCommand(
            Guid userId,
            string currentPassword,
            string newPassword)
        {
            UserId = userId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }

        public Guid UserId { get; }

        public string CurrentPassword { get; }

        public string NewPassword { get; }
    }
}
