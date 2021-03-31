using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Delegates;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Exceptions;
using CriThink.Server.Core.Interfaces;
using CriThink.Server.Core.Models.DTOs;
using CriThink.Server.Core.Models.LoginProviders;
using CriThink.Server.Providers.EmailSender.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CriThink.Server.Core.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtManager _jwtManager;
        private readonly IEmailSenderService _emailSender;
        private readonly IMapper _mapper;
        private readonly ILogger<IdentityService> _logger;
        private readonly ExternalLoginProviderResolver _externalLoginProviderResolver;

        public IdentityService(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IJwtManager jwtManager,
            IMapper mapper,
            IEmailSenderService emailSender,
            ILogger<IdentityService> logger,
            ExternalLoginProviderResolver externalLoginProviderResolver)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _externalLoginProviderResolver = externalLoginProviderResolver ?? throw new ArgumentNullException(nameof(externalLoginProviderResolver));
            _logger = logger;
        }

        public async Task<UserSignUpResponse> CreateNewUserAsync(UserSignUpRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var userCreationResult = await _userRepository.CreateUserAsync(user, request.Password).ConfigureAwait(false);
            if (!userCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(userCreationResult, "CreateNewUser");
                _logger?.LogError(ex, "Error creating a new user", user);
                throw ex;
            }

            var confirmationCode = await _userRepository.GetEmailConfirmationTokenAsync(user)
                .ConfigureAwait(false);

            await _emailSender.SendAccountConfirmationEmailAsync(user.Email, user.Id.ToString(), confirmationCode, user.UserName)
                .ConfigureAwait(false);

            return new UserSignUpResponse
            {
                UserId = user.Id.ToString(),
                UserEmail = user.Email,
                ConfirmationCode = confirmationCode
            };
        }

        public async Task<AdminSignUpResponse> CreateNewAdminAsync(AdminSignUpRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var adminUser = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var userCreationResult = await _userRepository.CreateUserAsync(adminUser, request.Password).ConfigureAwait(false);
            if (!userCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(userCreationResult);
                _logger?.LogError(ex, "Error creating a new admin", adminUser);
                throw ex;
            }

            var confirmationCode = await _userRepository.GetEmailConfirmationTokenAsync(adminUser)
                .ConfigureAwait(false);

            var emailConfirmationResult = await _userRepository.ConfirmUserEmailAsync(adminUser, confirmationCode).ConfigureAwait(false);
            if (!emailConfirmationResult.Succeeded)
            {
                var ex = new IdentityOperationException(emailConfirmationResult);
                _logger?.LogError(ex, "Error verifying user email", adminUser, confirmationCode);
                throw ex;
            }

            var roleResult = await _userRepository.AddUserToRoleAsync(adminUser, "Admin").ConfigureAwait(false);
            if (!roleResult.Succeeded)
            {
                var ex = new IdentityOperationException(emailConfirmationResult);
                _logger?.LogError(ex, "Error adding user to role", adminUser, confirmationCode);
                throw ex;
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, adminUser.Id.ToString()),
                new (ClaimTypes.Email, adminUser.Email),
                new (ClaimTypes.Name, adminUser.UserName),
                new (ClaimTypes.Role, "Admin"),
            };

            var claimsResult = await _userRepository.AddClaimsToUserAsync(adminUser, claims).ConfigureAwait(false);
            if (!claimsResult.Succeeded)
            {
                var ex = new IdentityOperationException(emailConfirmationResult);
                _logger?.LogError(ex, "Error adding user to role", adminUser, confirmationCode);
                throw ex;
            }

            var jwtToken = await _jwtManager.GenerateUserTokenAsync(adminUser).ConfigureAwait(false);

            return new AdminSignUpResponse
            {
                AdminId = adminUser.Id.ToString(),
                AdminEmail = adminUser.Email,
                JwtToken = jwtToken
            };
        }

        public Task<IList<RoleGetResponse>> GetRolesAsync()
        {
            return _roleRepository.GetAllRolesAsync();
        }

        public async Task CreateNewRoleAsync(SimpleRoleNameRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var role = new UserRole(request.Name);

            var roleCreationResult = await _roleRepository.CreateNewRoleAsync(role).ConfigureAwait(false);
            if (!roleCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(roleCreationResult);
                _logger?.LogError(ex, "Error creating a new role", role);
                throw ex;
            }
        }

        public async Task DeleteNewRoleAsync(SimpleRoleNameRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var role = await FindRoleAsync(request.Name).ConfigureAwait(false);
            if (role is null)
                throw new ResourceNotFoundException("The role doesn't exists", $"Name: '{request.Name}'");

            var roleDeletionResult = await _roleRepository.DeleteRoleAsync(role).ConfigureAwait(false);
            if (!roleDeletionResult.Succeeded)
            {
                var ex = new IdentityOperationException(roleDeletionResult);
                _logger?.LogError(ex, "Error removing a role", role);
                throw ex;
            }
        }

        public async Task UpdateRoleNameAsync(RoleUpdateNameRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var role = await FindRoleAsync(request.OldName).ConfigureAwait(false);
            if (role is null)
                throw new ResourceNotFoundException("The role doesn't exists", $"Name: '{request.OldName}'");

            role.Name = request.NewName;

            var roleRenamingResult = await _roleRepository.UpdateRoleAsync(role).ConfigureAwait(false);
            if (!roleRenamingResult.Succeeded)
            {
                var ex = new IdentityOperationException(roleRenamingResult);
                _logger?.LogError(ex, "Error renaming a role", role);
                throw ex;
            }
        }

        public async Task UpdateUserRoleAsync(UserRoleUpdateRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId);

            var role = await FindRoleAsync(request.Role).ConfigureAwait(false);
            if (role is null)
                throw new ResourceNotFoundException("The role is not valid", $"Role: '{request.Role}'");

            var currentUserRoles = await _userRepository.GetUserRolesAsync(user).ConfigureAwait(false);
            var areRoleRemoved = await _userRepository.RemoveUserFromRolesAsync(user, currentUserRoles).ConfigureAwait(false);
            if (!areRoleRemoved.Succeeded)
                throw new IdentityOperationException(areRoleRemoved);

            var isRoleAdded = await _userRepository.AddUserToRoleAsync(user, role.Name).ConfigureAwait(false);
            if (!isRoleAdded.Succeeded)
                throw new IdentityOperationException(isRoleAdded);
        }

        public async Task RemoveRoleFromUserAsync(UserRoleUpdateRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId);

            var role = await FindRoleAsync(request.Role).ConfigureAwait(false);
            if (role is null)
                throw new ResourceNotFoundException("The role is not valid", $"Role: '{request.Role}'");

            var areRoleRemoved = await _userRepository.RemoveUserFromRoleAsync(user, role.Name).ConfigureAwait(false);
            if (!areRoleRemoved.Succeeded)
                throw new IdentityOperationException(areRoleRemoved);
        }

        public async Task<UserGetAllResponse> GetAllUsersAsync(UserGetAllRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var pageIndex = request.PageIndex;
            var pageSize = request.PageSize;

            var allUsers = await _userRepository.GetAllUsersAsync(pageSize, pageIndex).ConfigureAwait(false);

            var userDtos = new List<UserGetResponse>();

            foreach (var user in allUsers.Take(pageSize))
            {
                var userDto = _mapper.Map<User, UserGetResponse>(user);
                var roles = await _userRepository.GetUserRolesAsync(user).ConfigureAwait(false);
                userDto.Roles = roles.ToList().AsReadOnly();
                userDtos.Add(userDto);
            }

            var response = new UserGetAllResponse(userDtos, allUsers.Count > pageSize);
            return response;
        }

        public async Task<UserGetDetailsResponse> GetUserByIdAsync(UserGetRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId);

            var userDto = _mapper.Map<User, UserGetDetailsResponse>(user);
            var roles = await _userRepository.GetUserRolesAsync(user).ConfigureAwait(false);
            userDto.Roles = roles.ToList().AsReadOnly();

            return userDto;
        }

        public async Task UpdateUserAsync(UserUpdateRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId);

            if (!string.IsNullOrWhiteSpace(request.UserName))
                user.UserName = request.UserName;

            if (request.IsEmailConfirmed != null)
                user.EmailConfirmed = request.IsEmailConfirmed.Value;

            if (request.AccessFailedCount != null)
                user.AccessFailedCount = request.AccessFailedCount.Value;

            if (request.IsLockoutEnabled != null)
                user.LockoutEnabled = request.IsLockoutEnabled.Value;

            if (request.LockoutEnd != null)
                user.LockoutEnd = request.LockoutEnd;

            await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
        }

        public async Task SoftDeleteUserAsync(UserGetRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId);

            user.IsDeleted = true;

            await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var user = await FindUserAsync(request.Email ?? request.UserName).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"Email: '{request.Email}' - Username: '{request.UserName}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var verificationResult = _userRepository.VerifyUserPassword(user, request.Password);
            await ProcessPasswordVerificationResultAsync(user, verificationResult).ConfigureAwait(false);

            var jwtToken = await _jwtManager.GenerateUserTokenAsync(user).ConfigureAwait(false);
            var response = new UserLoginResponse
            {
                UserId = user.Id.ToString(),
                UserEmail = user.Email,
                UserName = user.UserName,
                JwtToken = jwtToken
            };

            return response;
        }

        public async Task<ClaimsIdentity> LoginUserAsync(string emailOrUsername, string password, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername))
                throw new ArgumentNullException(nameof(emailOrUsername));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            var user = await FindUserAsync(emailOrUsername).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"EmailOrUsername: '{emailOrUsername}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.PasswordSignInAsync(user, password, rememberMe, false).ConfigureAwait(false);
            ProcessPasswordVerificationResult(result);

            var userClaims = await _userRepository.GetUserClaimsAsync(user).ConfigureAwait(false);
            return new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<VerifyUserEmailResponse> VerifyAccountEmailAsync(string userId, string confirmationCode)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"UserId: '{userId}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.ConfirmUserEmailAsync(user, confirmationCode).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error verifying user email", user, confirmationCode);
                throw ex;
            }

            var jwtToken = await _jwtManager.GenerateUserTokenAsync(user).ConfigureAwait(false);

            return new VerifyUserEmailResponse
            {
                UserId = userId,
                JwtToken = jwtToken,
                UserEmail = user.Email,
                Username = user.UserName
            };
        }

        public async Task ChangeUserPasswordAsync(string email, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentNullException(nameof(currentPassword));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var user = await FindUserAsync(email).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"User email: '{email}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.ChangeUserPasswordAsync(user, currentPassword, newPassword)
                .ConfigureAwait(false);

            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error changing user password", user);
                throw ex;
            }
        }

        public async Task GenerateUserPasswordTokenAsync(string email, string username)
        {
            if (string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException($"At least one between {email} and {username} must be provided");

            var user = await FindUserAsync(email ?? username).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"User email: '{email}' - username: '{username}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var token = await _userRepository.GenerateUserPasswordResetTokenAsync(user).ConfigureAwait(false);

            var encodedCode = Base64Helper.ToBase64(token);
            await _emailSender.SendPasswordResetEmailAsync(user.Email, user.Id.ToString(), encodedCode, username).ConfigureAwait(false);
        }

        public async Task ResetUserPasswordAsync(string userId, string token, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("The user doesn't exists", $"UserId: '{userId}'");
            if (user.IsDeleted)
                throw new InvalidOperationException("The user is disabled");

            var result = await _userRepository.ResetUserPasswordAsync(user, token, newPassword).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error resetting user password", user, token);
            }

            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error resetting user password", user);
                throw ex;
            }
        }

        public async Task<UserLoginResponse> ExternalProviderLoginAsync(ExternalLoginProviderRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.UserToken))
                throw new ArgumentException(nameof(request.UserToken));

            IExternalLoginProvider socialLoginProvider = _externalLoginProviderResolver(request.SocialProvider);

            var decodedToken = Base64Helper.FromBase64(request.UserToken);

            var userAccessInfo = await socialLoginProvider.GetUserAccessInfo(decodedToken).ConfigureAwait(false);

            var provider = request.SocialProvider.ToString().ToUpperInvariant();

            var currentUser = await _userRepository.FindUserByLoginAsync(provider, userAccessInfo.UserId).ConfigureAwait(false);
            if (currentUser is null)
            {
                currentUser = await CreateExternalProviderLoginUserAsync(userAccessInfo, provider).ConfigureAwait(false);
            }

            var jwtToken = await _jwtManager.GenerateUserTokenAsync(currentUser).ConfigureAwait(false);

            return new UserLoginResponse
            {
                JwtToken = jwtToken,
                UserEmail = currentUser.Email,
                UserId = currentUser.Id.ToString(),
                UserName = currentUser.UserName,
            };
        }

        public async Task<UsernameAvailabilityResponse> GetUsernameAvailabilityAsync(UsernameAvailabilityRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var user = await FindUserAsync(request.Username).ConfigureAwait(false);
            return new UsernameAvailabilityResponse
            {
                IsAvailable = user is null
            };
        }

        #region Privates

        private Task<User> FindUserAsync(string value)
        {
            return _userRepository.FindUserAsync(value);
        }

        private Task<UserRole> FindRoleAsync(string roleName)
        {
            return _roleRepository.FindRoleByNameAsync(roleName);
        }

        private async Task ProcessPasswordVerificationResultAsync(User user, PasswordVerificationResult verificationResult)
        {
            switch (verificationResult)
            {
                case PasswordVerificationResult.Failed:
                    throw new ResourceNotFoundException("Password is not correct");
                case PasswordVerificationResult.SuccessRehashNeeded:
                    await UpdateUserPasswordHashAsync(user).ConfigureAwait(false);
                    break;
                case PasswordVerificationResult.Success:
                    return;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void ProcessPasswordVerificationResult(SignInResult signInResult)
        {
            if (!signInResult.Succeeded)
                throw new ResourceNotFoundException("Password is not correct");
        }

        private async Task UpdateUserPasswordHashAsync(User user)
        {
            var result = await _userRepository.HasUserPasswordAsync(user).ConfigureAwait(false);
            if (!result)
            {
                var ex = new InvalidOperationException("Error hashing again user password");
                _logger?.LogError(ex, "Rehash needed but failed", user);
            }
        }

        private async Task<User> CreateExternalProviderLoginUserAsync(ExternalProviderUserInfo userAccessInfo, string providerName)
        {
            var user = new User
            {
                Email = userAccessInfo.Email,
                UserName = userAccessInfo.Username,
            };

            var userLoginInfo = new UserLoginInfo(providerName, userAccessInfo.UserId, providerName);

            var userCreated = await _userRepository.CreateUserAsync(user).ConfigureAwait(false);
            if (!userCreated.Succeeded)
            {
                var ex = new IdentityOperationException(userCreated);
                _logger?.LogError(ex, "Error creating user.", providerName, user);
                throw ex;
            }

            var loginAssociated = await _userRepository.AddUserLoginAsync(user, userLoginInfo)
                .ConfigureAwait(false);

            if (!loginAssociated.Succeeded)
            {
                var ex = new IdentityOperationException(loginAssociated);
                _logger?.LogError(ex, "Error associating external provider to user.", providerName, user);
                throw ex;
            }

            await _userRepository.SignInAsync(user, false).ConfigureAwait(false);

            return await _userRepository.FindUserByLoginAsync(providerName, userAccessInfo.UserId).ConfigureAwait(false);
        }

        #endregion
    }
}
