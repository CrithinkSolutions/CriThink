using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CriThink.Server.Core.Constants;
using CriThink.Server.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Infrastructure.Services
{
    internal class S3Service : IFileService
    {
        private readonly static RegionEndpoint Region = RegionEndpoint.EUCentral1;

        private readonly string _bucketName;
        private readonly ILogger<S3Service> _logger;

        public S3Service(IConfiguration configuration, ILogger<S3Service> logger)
        {
            _bucketName = configuration?["S3Settings:BucketName"] ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }

        public async Task<Uri> SaveFileAsync(IFormFile formFile, bool replaceIfExist, string fileName, params string[] paths)
        {
            try
            {
                await using var stream = formFile.OpenReadStream();
                return await UploadFileAsync(stream, fileName, paths);
            }
            catch (AmazonS3Exception ex)
            {
                _logger?.LogError(ex, "Error uploading avatar on S3 bucket using form file");
                throw;
            }
        }

        public async Task<Uri> SaveFileAsync(byte[] bytes, bool replaceIfExist, string fileName, params string[] paths)
        {
            try
            {
                await using var ms = new MemoryStream(bytes);
                return await UploadFileAsync(ms, fileName, paths);
            }
            catch (AmazonS3Exception ex)
            {
                _logger?.LogError(ex, "Error uploading avatar on S3 bucket using bytes");
                throw;
            }
        }

        public async Task DeleteFileAsync(string fileName, params string[] paths)
        {
            try
            {
                var s3Client = CreateS3Client();

                var pathWithRootFolder = BuildPathWithRootFolder(paths);
                var pathWithFileName = Path.Combine(pathWithRootFolder, fileName);
                var uriFormatLikePath = pathWithFileName.Replace("\\", "/");

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = uriFormatLikePath,
                };

                await s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception ex)
            {
                _logger?.LogError(ex, "Error deleting avatar on S3 bucket");
                throw;
            }
        }

        private async Task<Uri> UploadFileAsync(Stream stream, string fileName, string[] paths)
        {
            var fileTransferUtility = PrepareTransferUtility();
            var pathWithRootFolder = BuildPathWithRootFolder(paths);
            var pathWithFileName = Path.Combine(pathWithRootFolder, fileName);

            var uriFormatLikePath = pathWithFileName.Replace("\\", "/");

            await fileTransferUtility.UploadAsync(stream, _bucketName, uriFormatLikePath).ConfigureAwait(false);

            return GetResourceUri(uriFormatLikePath);
        }

        private static string BuildPathWithRootFolder(string[] paths)
        {
            var fullPath = BuildFullPath(paths);
            return PrependRootFolder(fullPath);
        }

        private static string BuildFullPath(string[] paths) =>
            Path.Combine(paths);

        public async Task<string> SaveUserAvatarAsync(IFormFile formFile, string subfolder, bool replaceIfExist = true)
        {
            try
            {
                var fileTransferUtility = PrepareTransferUtility();
                var destinationPath = PrependRootFolder(subfolder);

                await using var fileToUpload = formFile.OpenReadStream();
                await fileTransferUtility.UploadAsync(fileToUpload, _bucketName, destinationPath).ConfigureAwait(false);

                return $"{GetHostname()}{destinationPath}";
            }
            catch (AmazonS3Exception ex)
            {
                _logger?.LogError(ex, "Error uploading avatar on S3 bucket");
                return null;
            }
        }

        public async Task<string> SaveUserAvatarAsync(byte[] bytes, string subfolder, bool replaceIfExist = true)
        {
            try
            {
                var fileTransferUtility = PrepareTransferUtility();
                var destinationPath = PrependRootFolder(subfolder);

                await using var ms = new MemoryStream(bytes);
                await fileTransferUtility.UploadAsync(ms, _bucketName, destinationPath).ConfigureAwait(false);

                return $"{GetHostname()}{destinationPath}";
            }
            catch (AmazonS3Exception ex)
            {
                _logger?.LogError(ex, "Error uploading avatar on S3 bucket");
                return null;
            }
        }

        public async Task DeleteUserAvatarAsync(string subfolder)
        {
            try
            {
                var s3Client = CreateS3Client();

                var destinationPath = PrependRootFolder(subfolder);

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = destinationPath,
                };

                await s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception ex)
            {
                _logger?.LogError(ex, "Error deleting avatar on S3 bucket");
                throw;
            }
        }

        private static TransferUtility PrepareTransferUtility()
        {
            var s3Client = CreateS3Client();
            return new TransferUtility(s3Client);
        }

        private static AmazonS3Client CreateS3Client() =>
            new(Region);

        private static string PrependRootFolder(string pathWithFile) =>
            Path.Combine(AssetsConstants.RootFolder, pathWithFile);

        private Uri GetResourceUri(string pathWithFile) =>
            new($"{GetHostname()}{pathWithFile}");

        private string GetHostname() =>
            $"https://{_bucketName}.s3.{Region.SystemName}.{Region.PartitionDnsSuffix}/";
    }
}