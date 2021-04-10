﻿using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
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

        public async Task<string> SaveUserAvatarAsync(IFormFile formFile, string subfolder, bool replaceIfExist = true)
        {
            try
            {
                var fileTransferUtility = PrepareTransferUtility();
                var destinationPath = GetAvatarDestinationPath(subfolder);

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
                var destinationPath = GetAvatarDestinationPath(subfolder);

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

        private static TransferUtility PrepareTransferUtility()
        {
            var s3Client = new AmazonS3Client(Region);
            return new TransferUtility(s3Client);
        }

        private static string GetAvatarDestinationPath(string subfolder) =>
            $"{AssetsConstants.RootFolder}{subfolder}{AssetsConstants.AvatarFileName}";

        private string GetHostname() =>
            $"https://{_bucketName}.s3.{Region.SystemName}.{Region.PartitionDnsSuffix}/";
    }
}