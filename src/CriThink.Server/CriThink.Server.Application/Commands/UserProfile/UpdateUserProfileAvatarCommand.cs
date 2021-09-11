using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Updates the user avatar
    /// </summary>
    public class UpdateUserProfileAvatarCommand : IRequest
    {
        public UpdateUserProfileAvatarCommand(
            Guid userId,
            IFormFile formFile)
        {
            UserId = userId;
            FormFile = formFile;
        }

        public Guid UserId { get; }

        public IFormFile FormFile { get; }
    }
}
