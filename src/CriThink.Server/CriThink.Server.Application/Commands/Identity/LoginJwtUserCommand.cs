using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    public class LoginJwtUserCommand : IRequest<UserLoginResponse>
    {
        public LoginJwtUserCommand(
            string email,
            string username,
            string password)
        {
            Email = email;
            Username = username;
            Password = password;
        }

        public string Email { get; }

        public string Username { get; }

        public string Password { get; }
    }
}
