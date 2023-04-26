using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Domain.Constants;
using CriThink.Server.Domain.DomainServices;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Exceptions;
using CriThink.Server.Domain.QueryResults;
using CriThink.Server.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class IdentityQueries : IIdentityQueries
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentityQueries> _logger;

        public IdentityQueries(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IFileService fileService,
            IMapper mapper,
            ILogger<IdentityQueries> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _roleRepository = roleRepository ??
                throw new ArgumentNullException(nameof(roleRepository));

            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _logger = logger;
        }

        public async Task<UsernameAvailabilityResponse> AnyUserByUsernameAsync(string username)
        {
            _logger?.LogInformation(nameof(AnyUserByUsernameAsync));

            var user = await _userRepository.FindUserAsync(username);

            _logger?.LogInformation($"{nameof(AnyUserByUsernameAsync)}: done");

            return new UsernameAvailabilityResponse
            {
                IsAvailable = user is null
            };
        }

        public async Task<UserGetAllViewModel> GetAllUsersAsync(int pageSize, int pageIndex)
        {
            _logger?.LogInformation(nameof(GetAllUsersAsync));

            var allUsers = await _userRepository.GetAllUsersAsync(pageSize, pageIndex);

            var userDtos = new List<UserGetViewModel>();

            foreach (var user in allUsers.Take(pageSize))
            {
                var userDto = _mapper.Map<User, UserGetViewModel>(user);
                var roles = await _userRepository.GetUserRolesAsync(user).ConfigureAwait(false);
                userDto.Roles = roles.ToList().AsReadOnly();
                userDtos.Add(userDto);
            }

            var response = new UserGetAllViewModel(userDtos, allUsers.Count > pageSize);

            _logger?.LogInformation($"{nameof(GetAllUsersAsync)}: done");

            return response;
        }

        public async Task<IList<RoleGetViewModel>> GetAllRolesAsync()
        {
            _logger?.LogInformation(nameof(GetAllRolesAsync));

            var roles = await _roleRepository.GetAllRolesAsync();

            var response = _mapper.Map<IList<GetAllRolesQueryResult>, IList<RoleGetViewModel>>(roles);

            _logger?.LogInformation($"{nameof(GetAllRolesAsync)}: done");

            return response;
        }

        public async Task<UserGetDetailsViewModel> GetUserByIdAsync(Guid userId)
        {
            _logger?.LogInformation(nameof(GetUserByIdAsync));

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null)
                throw new CriThinkNotFoundException("User not found", userId);

            var userDto = _mapper.Map<User, UserGetDetailsViewModel>(user);
            var roles = await _userRepository.GetUserRolesAsync(user);
            userDto.Roles = roles.ToList().AsReadOnly();

            _logger?.LogInformation($"{nameof(GetUserByIdAsync)}: done");

            return userDto;
        }

        public async Task<UserProfileGetResponse> GetUserProfileByUserIdAsync(Guid userId)
        {
            _logger?.LogInformation(nameof(GetUserProfileByUserIdAsync));

            var user = await _userRepository.GetUserByIdAsync(userId);

            var response = _mapper.Map<UserProfile, UserProfileGetResponse>(user.Profile);

            response.AvatarPath = _fileService
                .GetAccessibleBlobUri(userId, ProfileConstants.AvatarFileName)
                .AbsoluteUri;

            _logger?.LogInformation($"{nameof(GetUserProfileByUserIdAsync)}: done", userId);

            return response;
        }

        public async Task<UserProfileGetAllRecentSearchResponse> GetUserRecentSearchesAsync(Guid userId)
        {
            _logger?.LogInformation(nameof(GetUserRecentSearchesAsync));

            var user = await _userRepository.GetUserByIdAsync(userId);

            var userLastSearches = user.Searches
                .OrderByDescending(s => s.Timestamp)
                .Take(10)
                .ToList();

            var response = _mapper.Map<IList<UserSearch>, IList<UserProfileGetRecentSearchResponse>>(userLastSearches);

            _logger?.LogInformation($"{nameof(GetUserRecentSearchesAsync)}: done", userId);

            return new UserProfileGetAllRecentSearchResponse
            {
                RecentSearches = response
            };
        }
    }
}
