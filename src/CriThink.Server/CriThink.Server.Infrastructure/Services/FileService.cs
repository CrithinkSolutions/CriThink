using System;
using System.IO;
using System.Threading.Tasks;
using CriThink.Server.Core.Constants;
using CriThink.Server.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Infrastructure.Services
{
    internal class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContext;

        public FileService(IWebHostEnvironment environment, IHttpContextAccessor httpContext)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        public async Task<string> SaveUserAvatarAsync(IFormFile formFile, string subfolder, bool replaceIfExist = true)
        {
            if (formFile == null || formFile.Length == 0)
                throw new ArgumentNullException(nameof(formFile));

            var destinationFolder = Path.Combine(_environment.WebRootPath, $"{AssetsConstants.RootFolder}{subfolder}");
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            var fullDestinationPath = Path.Combine(destinationFolder, AssetsConstants.AvatarFileName);
            if (!replaceIfExist &&
                File.Exists(fullDestinationPath))
                throw new InvalidOperationException("The file already exists");

            await using var fileStream = new FileStream(fullDestinationPath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);

            return $"{GetHostname()}{AssetsConstants.RootFolder}{subfolder}{AssetsConstants.AvatarFileName}";
        }

        private string GetHostname()
        {
            return $"{_httpContext?.HttpContext?.Request.Scheme}://{_httpContext?.HttpContext?.Request.Host}/";
        }
    }
}
