using System;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Core.Interfaces;

namespace CriThink.Server.Core.MapperProfiles
{
    internal class HostnameResolver : IMemberValueResolver<CriThink.Server.Core.Entities.UserProfile, UserProfileGetResponse, string, string>
    {
        private readonly IFileService _fileService;

        public HostnameResolver(IFileService fileService)
        {
            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));
        }

        public string Resolve(Entities.UserProfile source, UserProfileGetResponse destination, string sourceMember, string destMember, ResolutionContext context)
        {
            return $"{_fileService.Hostname}{sourceMember}";
        }
    }
}