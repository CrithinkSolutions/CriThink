using System;
using System.IO;
using System.Threading.Tasks;
using CriThink.Server.Domain.DomainServices;
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
            _environment = environment ??
                throw new ArgumentNullException(nameof(environment));

            _httpContext = httpContext ??
                throw new ArgumentNullException(nameof(httpContext));
        }

        public async Task<Uri> SaveFileAsync(
            IFormFile formFile,
            bool replaceIfExist,
            Guid userId,
            string fileName)
        {
            if (formFile is null || formFile.Length == 0)
                throw new ArgumentNullException(nameof(formFile));

            await using var fileStream = OpenStream(replaceIfExist, userId, fileName);

            await formFile.CopyToAsync(fileStream);

            return GetResourceUri(userId, fileName);
        }

        public async Task<Uri> SaveFileAsync(
            byte[] bytes,
            bool replaceIfExist,
            Guid userId,
            string fileName)
        {
            if (bytes is null || bytes.Length == 0)
                throw new ArgumentNullException(nameof(bytes));

            await using var fileStream = OpenStream(replaceIfExist, userId, fileName);

            await using var ms = new MemoryStream(bytes);
            ms.WriteTo(fileStream);

            return GetResourceUri(userId, fileName);
        }

        public Task DeleteFileAsync(Guid userId, string fileName)
        {
            var relativePath = BuildRelativePath(userId, fileName);
            var pathWithRootFolder = PrependRootFolder(relativePath);
            var pathWithFileName = Path.Combine(pathWithRootFolder, fileName);

            if (File.Exists(pathWithFileName))
                File.Delete(pathWithFileName);

            return Task.CompletedTask;
        }

        public Uri GetAccessibleBlobUri(Guid userId, string fileName)
        {
            var uri = GetResourceUri(userId, fileName);
            return uri;
        }

        private Stream OpenStream(bool replaceIfExist, Guid userId, string fileName)
        {
            var destinationFolder = PrependRootFolder(userId.ToString());

            EnsurePathExists(destinationFolder);

            var pathWithFile = Path.Combine(destinationFolder, fileName);

            var fileMode = replaceIfExist ?
                FileMode.Create :
                FileMode.CreateNew;

            return new FileStream(pathWithFile, fileMode);
        }

        private void EnsurePathExists(string fullPath)
        {
            var destinationFolder = PrependRootFolder(fullPath);
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);
        }

        private string PrependRootFolder(string fullPath) =>
            Path.Combine(_environment.WebRootPath, fullPath);

        private static string BuildRelativePath(Guid userId, string fileName) =>
            Path.Combine(userId.ToString(), fileName);

        private Uri GetResourceUri(Guid userId, string fileName) =>
            new($"{GetHostname()}{userId}/{fileName}");

        private string GetHostname() =>
            $"{_httpContext?.HttpContext?.Request.Scheme}://{_httpContext?.HttpContext?.Request.Host}/";
    }
}
