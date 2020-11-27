using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using IMapper = AutoMapper.IMapper;

namespace CriThink.Server.Core.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IEmailSenderService _emailSender;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;
        private readonly ILogger<IdentityService> _logger;
        private readonly ExternalLoginProviderResolver _externalLoginProviderResolver;
        private readonly SignInManager<User> _signInManager;

        public IdentityService(UserManager<User> userManager, RoleManager<UserRole> roleManager, IMapper mapper, IEmailSenderService emailSender, IConfiguration configuration, ILogger<IdentityService> logger, ExternalLoginProviderResolver externalLoginProviderResolver, SignInManager<User> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger;
            _jwtTokenHandler = new JwtSecurityTokenHandler();
            _externalLoginProviderResolver = externalLoginProviderResolver ?? throw new ArgumentNullException(nameof(externalLoginProviderResolver));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<UserSignUpResponse> CreateNewUserAsync(UserSignUpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var userCreationResult = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(false);
            if (!userCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(userCreationResult);
                _logger?.LogError(ex, "Error creating a new user", user);
                throw ex;
            }

            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user)
                .ConfigureAwait(false);

            var encodedCode = Base64Helper.ToBase64(confirmationCode);

            await _emailSender.SendAccountConfirmationEmailAsync(user.Email, user.Id.ToString(), encodedCode)
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
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var adminUser = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var userCreationResult = await _userManager.CreateAsync(adminUser, request.Password).ConfigureAwait(false);
            if (!userCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(userCreationResult);
                _logger?.LogError(ex, "Error creating a new admin", adminUser);
                throw ex;
            }

            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser)
                .ConfigureAwait(false);
            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(adminUser, confirmationCode).ConfigureAwait(false);
            if (!emailConfirmationResult.Succeeded)
            {
                var ex = new IdentityOperationException(emailConfirmationResult);
                _logger?.LogError(ex, "Error verifying user email", adminUser, confirmationCode);
                throw ex;
            }

            var roleResult = await _userManager.AddToRoleAsync(adminUser, "Admin").ConfigureAwait(false);
            if (!roleResult.Succeeded)
            {
                var ex = new IdentityOperationException(emailConfirmationResult);
                _logger?.LogError(ex, "Error adding user to role", adminUser, confirmationCode);
                throw ex;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, adminUser.Id.ToString()),
                new Claim(ClaimTypes.Email, adminUser.Email),
                new Claim(ClaimTypes.Name, adminUser.UserName),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var claimsResult = await _userManager.AddClaimsAsync(adminUser, claims).ConfigureAwait(false);
            if (!claimsResult.Succeeded)
            {
                var ex = new IdentityOperationException(emailConfirmationResult);
                _logger?.LogError(ex, "Error adding user to role", adminUser, confirmationCode);
                throw ex;
            }

            var jwtToken = await GenerateTokenAsync(adminUser).ConfigureAwait(false);

            return new AdminSignUpResponse
            {
                AdminId = adminUser.Id.ToString(),
                AdminEmail = adminUser.Email,
                JwtToken = jwtToken
            };
        }

        public async Task<IList<RoleGetResponse>> GetRolesAsync()
        {
            var allRoles = await _roleManager.Roles
                .Select(r => new RoleGetResponse
                {
                    Name = r.Name,
                    Id = r.Id.ToString()
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return allRoles;
        }

        public async Task CreateNewRoleAsync(SimpleRoleNameRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var role = new UserRole(request.Name);

            var roleCreationResult = await _roleManager.CreateAsync(role).ConfigureAwait(false);
            if (!roleCreationResult.Succeeded)
            {
                var ex = new IdentityOperationException(roleCreationResult);
                _logger?.LogError(ex, "Error creating a new role", role);
                throw ex;
            }
        }

        public async Task DeleteNewRoleAsync(SimpleRoleNameRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var role = await FindRoleAsync(request.Name).ConfigureAwait(false);
            if (role == null)
                throw new ResourceNotFoundException("The role doesn't exists", $"Name: '{request.Name}'");

            var roleDeletionResult = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
            if (!roleDeletionResult.Succeeded)
            {
                var ex = new IdentityOperationException(roleDeletionResult);
                _logger?.LogError(ex, "Error removing a role", role);
                throw ex;
            }
        }

        public async Task UpdateRoleNameAsync(RoleUpdateNameRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var role = await FindRoleAsync(request.OldName).ConfigureAwait(false);
            if (role == null)
                throw new ResourceNotFoundException("The role doesn't exists", $"Name: '{request.OldName}'");

            role.Name = request.NewName;

            var roleRenamingResult = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
            if (!roleRenamingResult.Succeeded)
            {
                var ex = new IdentityOperationException(roleRenamingResult);
                _logger?.LogError(ex, "Error renaming a role", role);
                throw ex;
            }
        }

        public async Task UpdateUserRoleAsync(UserRoleUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user is null)
                throw new ResourceNotFoundException("User not found", userId);

            var role = await FindRoleAsync(request.Role).ConfigureAwait(false);
            if (role == null)
                throw new ResourceNotFoundException("The role is not valid", $"Role: '{request.Role}'");

            var currentUserRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var areRoleRemoved = await _userManager.RemoveFromRolesAsync(user, currentUserRoles).ConfigureAwait(false);
            if (!areRoleRemoved.Succeeded)
                throw new IdentityOperationException(areRoleRemoved);

            var isRoleAdded = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
            if (!isRoleAdded.Succeeded)
                throw new IdentityOperationException(isRoleAdded);
        }

        public async Task RemoveRoleFromUserAsync(UserRoleUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("User not found", userId);

            var role = await FindRoleAsync(request.Role).ConfigureAwait(false);
            if (role == null)
                throw new ResourceNotFoundException("The role is not valid", $"Role: '{request.Role}'");

            var areRoleRemoved = await _userManager.RemoveFromRoleAsync(user, role.Name).ConfigureAwait(false);
            if (!areRoleRemoved.Succeeded)
                throw new IdentityOperationException(areRoleRemoved);
        }

        public async Task<UserGetAllResponse> GetAllUsersAsync(UserGetAllRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var pageIndex = request.PageIndex;
            var pageSize = request.PageSize;

            var allUsers = await _userManager.Users
                .AsQueryable()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize + 1)
                .ToListAsync()
                .ConfigureAwait(false);

            var userDtos = new List<UserGetResponse>();

            foreach (var user in allUsers.Take(pageSize))
            {
                var userDto = _mapper.Map<User, UserGetResponse>(user);
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDto.Roles = roles.ToList().AsReadOnly();
                userDtos.Add(userDto);
            }

            var response = new UserGetAllResponse(userDtos, allUsers.Count > pageSize);
            return response;
        }

        public async Task<UserGetDetailsResponse> GetUserByIdAsync(UserGetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("User not found", userId);

            var userDto = _mapper.Map<User, UserGetDetailsResponse>(user);
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            userDto.Roles = roles.ToList().AsReadOnly();

            return userDto;
        }

        public async Task UpdateUserAsync(UserUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
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

            await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task SoftDeleteUserAsync(UserGetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("User not found", userId);

            user.IsDeleted = true;

            await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task DeleteUserAsync(UserGetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("User not found", userId);

            await _userManager.DeleteAsync(user).ConfigureAwait(false);
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await FindUserAsync(request.Email ?? request.UserName).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"Email: '{request.Email}' - Username: '{request.UserName}'");

            var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            await ProcessPasswordVerificationResultAsync(user, verificationResult).ConfigureAwait(false);

            var jwtToken = await GenerateTokenAsync(user).ConfigureAwait(false);
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
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"EmailOrUsername: '{emailOrUsername}'");

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false).ConfigureAwait(false);
            ProcessPasswordVerificationResult(result);

            var userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            return new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<VerifyUserEmailResponse> VerifyAccountEmailAsync(string userId, string confirmationCode)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"UserId: '{userId}'");

            var result = await _userManager.ConfirmEmailAsync(user, confirmationCode).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error verifying user email", user, confirmationCode);
                throw ex;
            }

            var jwtToken = await GenerateTokenAsync(user).ConfigureAwait(false);

            return new VerifyUserEmailResponse
            {
                UserId = userId,
                JwtToken = jwtToken,
                UserEmail = user.Email,
                Username = user.UserName
            };
        }

        public async Task<bool> ChangeUserPasswordAsync(string email, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentNullException(nameof(currentPassword));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"User email: '{email}'");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword)
                .ConfigureAwait(false);

            if (result.Succeeded)
                return true;

            var ex = new IdentityOperationException(result);
            _logger?.LogError(ex, "Error changing user password", user);
            throw ex;
        }

        public async Task GenerateUserPasswordTokenAsync(string email, string username)
        {
            if (string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException($"At least one between {email} and {username} must be provided");

            var user = await FindUserAsync(email ?? username).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"User email: '{email}' - username: '{username}'");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

            var encodedCode = Base64Helper.ToBase64(token);
            await _emailSender.SendPasswordResetEmailAsync(user.Email, user.Id.ToString(), encodedCode).ConfigureAwait(false);
        }

        public async Task<bool> ResetUserPasswordAsync(string userId, string token, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"UserId: '{userId}'");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error resetting user password", user, token);
            }

            return result.Succeeded;
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

            var currentUser = await _userManager.FindByLoginAsync(provider, userAccessInfo.UserId).ConfigureAwait(false);

            if (currentUser is null)
            {
                currentUser = await CreateExternalProviderLoginUser(userAccessInfo, provider).ConfigureAwait(false);
            }

            var jwtToken = await GenerateTokenAsync(currentUser).ConfigureAwait(false);

            return new UserLoginResponse
            {
                JwtToken = jwtToken,
                UserEmail = currentUser.Email,
                UserId = currentUser.Id.ToString(),
                UserName = currentUser.UserName,
            };
        }

        #region Privates

        private async Task<User> FindUserAsync(string value)
        {
            if (EmailHelper.IsEmail(value))
            {
                return await _userManager.FindByEmailAsync(value).ConfigureAwait(false);
            }

            if (Guid.TryParse(value, out _))
            {
                return await _userManager.FindByIdAsync(value).ConfigureAwait(false);
            }

            return await _userManager.FindByNameAsync(value).ConfigureAwait(false);
        }

        private async Task<UserRole> FindRoleAsync(string roleName = "", string roleId = "")
        {
            if (!string.IsNullOrWhiteSpace(roleName))
                return await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(roleId))
                return await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);

            return null;
        }

        private async Task<JwtTokenResponse> GenerateTokenAsync(User user)
        {
            var secretKey = _configuration["Jwt-SecretKey"];
            var audience = _configuration["Jwt-Audience"];
            var issuer = _configuration["Jwt-Issuer"];
            var expirationFromNow = _configuration["Jwt-ExpirationInHours"];

            var hasExpiration = double.TryParse(expirationFromNow, out var expirationInHours);
            if (!hasExpiration)
            {
                expirationInHours = 0.5;
                _logger?.LogCritical(new SecretNotFoundException("Token duration.", nameof(IdentityService)), "Used default token duration.");
            }

            // Get user claims
            var claims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            // Get user role's claims
            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            foreach (var userRole in userRoles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole));

            var token = new JwtBuilder()
                .AddAudience(audience)
                .AddClaims(claims)
                .AddIssuer(issuer)
                .AddSecurityKey(signingKey)
                .AddExpireDate(expirationInHours)
                .AddSubject(user.Email)
                .Build();

            var tokenString = _jwtTokenHandler.WriteToken(token);
            return new JwtTokenResponse
            {
                ExpirationDate = token.ValidTo,
                Token = tokenString
            };
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
            var result = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
            if (!result)
            {
                var ex = new InvalidOperationException("Error hashing again user password");
                _logger?.LogError(ex, "Rehash needed but failed", user);
            }
        }

        private async Task<User> CreateExternalProviderLoginUser(ExternalProviderUserInfo userAccessInfo, string providerName)
        {
            var user = new User
            {
                Email = userAccessInfo.Email,
                UserName = userAccessInfo.Username,
            };

            var userLoginInfo = new UserLoginInfo(providerName, userAccessInfo.UserId, providerName);

            var userCreated = await _userManager.CreateAsync(user).ConfigureAwait(false);
            if (userCreated.Succeeded)
            {
                var loginAssociated = await _userManager.AddLoginAsync(user, userLoginInfo).ConfigureAwait(false);

                if (loginAssociated.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false).ConfigureAwait(false);
                }
                else
                {
                    var ex = new IdentityOperationException(loginAssociated);
                    _logger?.LogError(ex, "Error associating external provider to user.", providerName, user);
                    throw ex;
                }
            }
            else
            {
                var ex = new IdentityOperationException(userCreated);
                _logger?.LogError(ex, "Error creating user.", providerName, user);
                throw ex;
            }

            return await _userManager.FindByLoginAsync(providerName, userAccessInfo.UserId).ConfigureAwait(false);
        }

        #endregion
    }
}
