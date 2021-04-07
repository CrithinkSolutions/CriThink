using System;
using System.IO;
using System.Threading.Tasks;
using CriThink.Server.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Infrastructure.Services
{
    internal class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task SaveUserAvatarAsync(IFormFile formFile, string subfolder, bool replaceIfExist = true)
        {
            if (formFile == null || formFile.Length == 0)
                throw new ArgumentNullException(nameof(formFile));

            var destinationFolder = Path.Combine(_environment.WebRootPath, $"uploads/{subfolder}");
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            var fullDestinationPath = Path.Combine(destinationFolder, "avatar.jpg");
            if (!replaceIfExist &&
                File.Exists(fullDestinationPath))
                throw new InvalidOperationException("The file already exists");

            await using var fileStream = new FileStream(fullDestinationPath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);
        }
    }

    internal class S3Service : IFileService
    {
        public async Task SaveUserAvatarAsync(IFormFile formFile, string subfolder, bool replaceIfExist = true)
        {
            throw new NotImplementedException();
        }
    }
}
