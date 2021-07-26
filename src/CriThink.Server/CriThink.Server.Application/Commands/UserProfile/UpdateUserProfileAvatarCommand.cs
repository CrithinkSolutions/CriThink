using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Application.Commands
{
    /// <summary>
    /// Updates the user avatar
    /// </summary>
    public class UpdateUserProfileAvatarCommand : IRequest
    {
        public UpdateUserProfileAvatarCommand(IFormFile formFile)
        {
            FormFile = formFile;
        }

        public IFormFile FormFile { get; }
    }
}
