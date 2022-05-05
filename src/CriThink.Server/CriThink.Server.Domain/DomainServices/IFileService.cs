using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Domain.DomainServices
{
    public interface IFileService
    {
        /// <summary>
        /// Save file on storage
        /// </summary>
        /// <param name="formFile">The file to save</param>
        /// <param name="replaceIfExist">True if it shall replace the existing file</param>
        /// <param name="userId">User id</param>
        /// <param name="fileName">The file name</param>
        /// <returns>The resource URI</returns>
        Task<Uri> SaveFileAsync(IFormFile formFile, bool replaceIfExist, Guid userId, string fileName);

        /// <summary>
        /// Save file on storage
        /// </summary>
        /// <param name="bytes">The file bytes</param>
        /// <param name="replaceIfExist">True if it shall replace the existing file</param>
        /// <param name="userId">User id</param>
        /// <param name="fileName">The file name</param>
        /// <returns>The resource URI</returns>
        Task<Uri> SaveFileAsync(byte[] bytes, bool replaceIfExist, Guid userId, string fileName);

        /// <summary>
        /// Get absolute URI of the asked resource with a valid permission
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="fileName">The file name</param>
        /// <returns>The resource URI with granted permissions</returns>
        Uri GetAccessibleBlobUri(Guid userId, string fileName);

        /// <summary>
        /// Delete the given file from storage
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="fileName">The file name</param>
        /// <returns></returns>
        Task DeleteFileAsync(Guid userId, string fileName);
    }
}
