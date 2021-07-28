using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Application.Commands
{
    public class CreateUserCommand : IRequest<UserSignUpResponse>
    {
        public CreateUserCommand(
            string username,
            string email,
            string password,
            IFormFile formFile)
        {
            Username = username;
            Email = email;
            Password = password;
            FormFile = formFile;
        }

        public string Username { get; }

        public string Email { get; }

        public string Password { get; }

        public IFormFile FormFile { get; }
    }
}
