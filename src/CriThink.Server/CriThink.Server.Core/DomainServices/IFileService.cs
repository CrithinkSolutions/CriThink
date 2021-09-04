using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Core.DomainServices
{
    public interface IFileService
    {
        /// <summary>
        /// Save file on storage
        /// </summary>
        /// <param name="formFile">The file to save</param>
        /// <param name="replaceIfExist">True if it shall replace the existing file</param>
        /// <param name="fileName">The file name</param>
        /// <param name="paths">The destination paths</param>
        /// <returns>The resource URI</returns>
        Task<Uri> SaveFileAsync(IFormFile formFile, bool replaceIfExist, string fileName, params string[] paths);

        /// <summary>
        /// Save file on storage
        /// </summary>
        /// <param name="bytes">The file bytes</param>
        /// <param name="replaceIfExist">True if it shall replace the existing file</param>
        /// <param name="fileName">The file name</param>
        /// <param name="paths">The destination paths</param>
        /// <returns>The resource URI</returns>
        Task<Uri> SaveFileAsync(byte[] bytes, bool replaceIfExist, string fileName, params string[] paths);

        /// <summary>
        /// Delete the given file from storage
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="paths">The file path with file name</param>
        /// <returns></returns>
        Task DeleteFileAsync(string fileName, params string[] paths);
    }
}
