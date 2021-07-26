using System;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class UserProfileQueries : IUserProfileQueries
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IHttpContextAccessor _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserProfileQueries> _logger;

        public UserProfileQueries(
            IUserProfileRepository userProfileRepo,
            IHttpContextAccessor context,
            IMapper mapper,
            ILogger<UserProfileQueries> logger)
        {
            _userProfileRepo = userProfileRepo ??
                throw new ArgumentNullException(nameof(userProfileRepo));

            _context = context ??
                throw new ArgumentNullException(nameof(context));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _logger = logger;
        }

        public async Task<UserProfileGetResponse> GetUserProfileAsync()
        {
            _logger?.LogInformation("Get UserProfile");

            var userId = _context.HttpContext.User.GetId();

            var entity = await _userProfileRepo.GetUserProfileByUserIdAsync(userId);

            var response = _mapper.Map<UserProfile, UserProfileGetResponse>(entity);

            _logger?.LogInformation("Get UserProfile: done", userId);

            return response;
        }
    }
}
