using System;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Domain.DomainServices;

namespace CriThink.Server.Application.Automapper
{
    internal class HostnameResolver : IMemberValueResolver<Domain.Entities.UserProfile, UserProfileGetResponse, string, string>
    {
        private readonly IFileService _fileService;

        public HostnameResolver(IFileService fileService)
        {
            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));
        }

        public string Resolve(
            Domain.Entities.UserProfile source,
            UserProfileGetResponse destination,
            string sourceMember,
            string destMember,
            ResolutionContext context)
        {
            if (_fileService.Hostname.EndsWith("/") &&
                sourceMember.StartsWith("/"))
            {
                sourceMember = sourceMember.Substring(1);
            }

            return $"{_fileService.Hostname}{sourceMember}";
        }
    }
}
