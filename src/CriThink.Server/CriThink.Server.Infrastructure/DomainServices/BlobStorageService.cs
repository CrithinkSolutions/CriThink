using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using CriThink.Server.Domain.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.DomainServices
{
    public class BlobStorageService : IFileService
    {
        private static BlobServiceClient BlobServiceClient;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(
            IConfiguration configuration,
            ILogger<BlobStorageService> logger)
        {
            if (BlobServiceClient is null)
            {
                string cs = configuration.GetConnectionString("InternalStorageAccount");
                BlobServiceClient = new BlobServiceClient(cs);
            }

            _logger = logger;
        }

        public async Task DeleteFileAsync(Guid userId, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            try
            {
                var container = GetUserBlobContainer(userId);
                await container.DeleteBlobAsync(fileName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting blob on Azure Account Storage");
                throw;
            }
        }

        public Uri GetAccessibleBlobUri(Guid userId, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            try
            {
                var container = GetUserBlobContainer(userId);
                BlobClient blobClient = container.GetBlobClient(fileName);
                if (!blobClient.Exists())
                    throw new InvalidOperationException("The specified blob doesn't exist");

                var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(3));
                return sasUri;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting blob on Azure Account Storage");
                throw;
            }
        }

        public async Task<Uri> SaveFileAsync(
            IFormFile formFile,
            bool replaceIfExist,
            Guid userId,
            string fileName)
        {
            if (formFile is null)
                throw new ArgumentNullException(nameof(formFile));

            try
            {
                await using var stream = formFile.OpenReadStream();
                return await UploadFileAsync(stream, userId, fileName, replaceIfExist);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error uploading blob on Azure Account Storage using form file");
                throw;
            }

        }

        public async Task<Uri> SaveFileAsync(
            byte[] bytes,
            bool replaceIfExist,
            Guid userId,
            string fileName)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            try
            {
                await using var ms = new MemoryStream(bytes);
                return await UploadFileAsync(ms, userId, fileName, replaceIfExist);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error uploading blob on Azure Account Storage using bytes");
                throw;
            }
        }

        private static async Task<Uri> UploadFileAsync(
            Stream stream,
            Guid userId,
            string blobName,
            bool replaceIfExist)
        {
            var container = GetUserBlobContainer(userId);

            await container.CreateIfNotExistsAsync();

            BlobClient blobClient = container.GetBlobClient(blobName);

            if (blobClient.Exists() && replaceIfExist)
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }
            else
            {
                await container.UploadBlobAsync(blobName, stream);
            }

            blobClient = container.GetBlobClient(blobName);
            return blobClient.Uri;
        }

        private static BlobContainerClient GetUserBlobContainer(Guid userId) =>
            BlobServiceClient.GetBlobContainerClient(userId.ToString());
    }
}
