using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Core.Interfaces
{
    public interface IFileService
    {
        Task SaveUserAvatarAsync(IFormFile formFile, string subfolder, bool replaceIfExist = true);
    }
}
