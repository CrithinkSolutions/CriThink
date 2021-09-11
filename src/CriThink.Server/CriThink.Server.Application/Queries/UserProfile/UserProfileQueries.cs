using System;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class UserProfileQueries : IUserProfileQueries
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserProfileQueries> _logger;

        public UserProfileQueries(
            IUserProfileRepository userProfileRepo,
            IMapper mapper,
            ILogger<UserProfileQueries> logger)
        {
            _userProfileRepo = userProfileRepo ??
                throw new ArgumentNullException(nameof(userProfileRepo));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _logger = logger;
        }

        public async Task<UserProfileGetResponse> GetUserProfileByUserIdAsync(Guid userId)
        {
            _logger?.LogInformation(nameof(GetUserProfileByUserIdAsync));

            var entity = await _userProfileRepo.GetUserProfileByUserIdAsync(userId);

            var response = _mapper.Map<UserProfile, UserProfileGetResponse>(entity);

            _logger?.LogInformation($"{nameof(GetUserProfileByUserIdAsync)}: done", userId);

            return response;
        }
    }
}
