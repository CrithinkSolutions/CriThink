using System.Security.Claims;
using MediatR;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Login user
    /// </summary>
    public class LoginCookieUserCommand : IRequest<ClaimsIdentity>
    {
        public LoginCookieUserCommand(string email, string username, string password, bool rememberMe)
        {
            Email = email;
            Username = username;
            Password = password;
            RememberMe = rememberMe;
        }

        public string Email { get; }

        public string Username { get; }

        public string Password { get; }

        public bool RememberMe { get; }
    }
}
