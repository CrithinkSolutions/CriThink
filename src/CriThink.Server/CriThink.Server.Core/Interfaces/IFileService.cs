using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Core.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Save the user avatar on storage
        /// </summary>
        /// <param name="formFile">The avatar uploaded</param>
        /// <param name="subfolder">The destination path</param>
        /// <param name="replaceIfExist">(Optional) True if shall replace the existing file</param>
        /// <returns>The URI where the resource is located</returns>
        Task<string> SaveUserAvatarAsync(IFormFile formFile, string subfolder, bool replaceIfExist = true);

        /// <summary>
        /// Save the user avatar on storage
        /// </summary>
        /// <param name="bytes">The image's bytes</param>
        /// <param name="subfolder">The destination path</param>
        /// <param name="replaceIfExist">(Optional) True if shall replace the existing file</param>
        /// <returns>The URI where the resource is located</returns>
        Task<string> SaveUserAvatarAsync(byte[] bytes, string subfolder, bool replaceIfExist = true);
    }
}
