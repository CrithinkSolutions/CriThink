using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Application.Administration.ViewModels;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.QueryResults;
using CriThink.Server.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Application.Queries
{
    internal class IdentityQueries : IIdentityQueries
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentityQueries> _logger;

        public IdentityQueries(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            ILogger<IdentityQueries> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            _roleRepository = roleRepository ??
                throw new ArgumentNullException(nameof(roleRepository));

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
                throw new ResourceNotFoundException("User not found", userId);

            var userDto = _mapper.Map<User, UserGetDetailsViewModel>(user);
            var roles = await _userRepository.GetUserRolesAsync(user);
            userDto.Roles = roles.ToList().AsReadOnly();

            _logger?.LogInformation($"{nameof(GetUserByIdAsync)}: done");

            return userDto;
        }
    }
}
