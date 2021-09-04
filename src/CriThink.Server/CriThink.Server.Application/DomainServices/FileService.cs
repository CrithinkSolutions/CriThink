using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Core.Constants;
using CriThink.Server.Core.DomainServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CriThink.Server.Application.DomainServices
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

        public async Task<Uri> SaveFileAsync(IFormFile formFile, bool replaceIfExist, string fileName, params string[] paths)
        {
            if (formFile is null || formFile.Length == 0)
                throw new ArgumentNullException(nameof(formFile));

            if (paths is null || !paths.Any())
                throw new ArgumentNullException(nameof(paths));

            var fullRelativePath = BuildFullRelativePath(paths);

            await using var fileStream = OpenStream(replaceIfExist, fileName, fullRelativePath);

            await formFile.CopyToAsync(fileStream);

            return GetResourceUri(fullRelativePath, fileName);
        }

        public async Task<Uri> SaveFileAsync(byte[] bytes, bool replaceIfExist, string fileName, params string[] paths)
        {
            if (bytes is null || bytes.Length == 0)
                throw new ArgumentNullException(nameof(bytes));

            if (paths is null || !paths.Any())
                throw new ArgumentNullException(nameof(paths));

            var fullRelativePath = BuildFullRelativePath(paths);

            await using var fileStream = OpenStream(replaceIfExist, fileName, fullRelativePath);

            await using var ms = new MemoryStream(bytes);
            ms.WriteTo(fileStream);

            return GetResourceUri(fullRelativePath, fileName);
        }

        public Task DeleteFileAsync(string fileName, params string[] paths)
        {
            if (paths is null || !paths.Any())
                throw new ArgumentNullException(nameof(paths));

            var fullPath = BuildFullRelativePath(paths);
            var pathWithRootFolder = PrependRootFolder(fullPath);
            var pathWithFileName = Path.Combine(pathWithRootFolder, fileName);

            if (File.Exists(pathWithFileName))
                File.Delete(pathWithFileName);

            return Task.CompletedTask;
        }

        private Stream OpenStream(bool replaceIfExist, string fileName, string fullRelativePath)
        {
            var destinationFolder = PrependRootFolder(fullRelativePath);

            EnsurePathExists(destinationFolder);

            var pathWithFile = Path.Combine(destinationFolder, fileName);

            var fileStream = InitializeStream(replaceIfExist, pathWithFile);
            return fileStream;
        }

        private void EnsurePathExists(string fullPath)
        {
            var destinationFolder = PrependRootFolder(fullPath);
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);
        }

        private string PrependRootFolder(string fullPath) =>
            Path.Combine(_environment.WebRootPath, fullPath);

        private static Stream InitializeStream(bool replaceIfExist, string pathWithFile)
        {
            var fileMode = replaceIfExist ?
                FileMode.Create :
                FileMode.CreateNew;

            return new FileStream(pathWithFile, fileMode);
        }

        private static string BuildFullRelativePath(string[] paths) =>
            Path.Combine(AssetsConstants.RootFolder, Path.Combine(paths));

        private Uri GetResourceUri(string pathWithFile, string fileName) =>
            new($"{GetHostname()}{pathWithFile}{fileName}");

        private string GetHostname() =>
            $"{_httpContext?.HttpContext?.Request.Scheme}://{_httpContext?.HttpContext?.Request.Host}/";
    }
}
