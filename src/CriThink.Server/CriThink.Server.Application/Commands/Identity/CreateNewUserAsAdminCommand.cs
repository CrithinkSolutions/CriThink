using CriThink.Common.Endpoints.DTOs.Admin;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Create a new user with admin role
    /// </summary>
    public class CreateNewUserAsAdminCommand : IRequest
    {
        public CreateNewUserAsAdminCommand(
            string email,
            string userName,
            string password)
        {
            Email = email;
            Username = userName;
            Password = password;
        }

        public string Email { get; }

        public string Username { get; }

        public string Password { get; }
    }
}
