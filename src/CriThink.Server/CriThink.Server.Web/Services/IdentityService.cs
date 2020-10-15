﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CriThink.Common.Endpoints.DTOs.Admin;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Entities;
using CriThink.Server.Providers.EmailSender.Services;
using CriThink.Server.Web.Exceptions;
using CriThink.Server.Web.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CriThink.Server.Web.Services
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

        public IdentityService(UserManager<User> userManager, RoleManager<UserRole> roleManager, IMapper mapper, IEmailSenderService emailSender, IConfiguration configuration, ILogger<IdentityService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger;
            _jwtTokenHandler = new JwtSecurityTokenHandler();
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

            //var roleRenamingResult = await _roleManager.SetRoleNameAsync(role, request.NewName).ConfigureAwait(false);
            var roleRenamingResult = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
            if (!roleRenamingResult.Succeeded)
            {
                var ex = new IdentityOperationException(roleRenamingResult);
                _logger?.LogError(ex, "Error renaming a role", role);
                throw ex;
            }
        }

        public async Task<IList<UserGetAllResponse>> GetAllUsersAsync(int pageSize, int pageIndex)
        {
            var allUsers = await _userManager.Users
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            var dtos = _mapper.Map<IList<User>, IList<UserGetAllResponse>>(allUsers);
            return dtos;
        }

        public async Task<UserGetResponse> GetUserByIdAsync(UserGetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId: userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("User not found", userId);

            return _mapper.Map<User, UserGetResponse>(user);
        }

        public async Task UpdateUserAsync(UserUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var userId = request.UserId.ToString();

            var user = await FindUserAsync(userId: userId).ConfigureAwait(false);
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

            var user = await FindUserAsync(userId: userId).ConfigureAwait(false);
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

            var user = await FindUserAsync(userId: userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("User not found", userId);

            await _userManager.DeleteAsync(user).ConfigureAwait(false);
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await FindUserAsync(email: request.Email, userName: request.UserName).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"Email: '{request.Email}' - Username: '{request.UserName}'");

            var verificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            await ProcessPasswordVerificationResult(user, verificationResult).ConfigureAwait(false);

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

        public async Task<VerifyUserEmailResponse> VerifyAccountEmailAsync(string userId, string confirmationCode)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(confirmationCode))
                throw new ArgumentNullException(nameof(confirmationCode));

            var user = await FindUserAsync(userId: userId).ConfigureAwait(false);
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
                UserEmail = user.Email
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

            var user = await FindUserAsync(email, username).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"User email: '{email}' - username: '{username}'");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

            var encodedCode = Base64Helper.ToBase64(token);
            await _emailSender.SendPasswordResetEmailAsync(user.Email, user.Id.ToString(), encodedCode).ConfigureAwait(false);
        }

        public async Task<VerifyUserEmailResponse> ResetUserPasswordAsync(string userId, string token, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var user = await FindUserAsync(userId: userId).ConfigureAwait(false);
            if (user == null)
                throw new ResourceNotFoundException("The user doesn't exists", $"UserId: '{userId}'");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                var ex = new IdentityOperationException(result);
                _logger?.LogError(ex, "Error resetting user password", user, token);
                throw ex;
            }

            var jwtToken = await GenerateTokenAsync(user).ConfigureAwait(false);

            return new VerifyUserEmailResponse
            {
                UserId = userId,
                JwtToken = jwtToken,
                UserEmail = user.Email
            };
        }

        #region Privates

        private async Task<User> FindUserAsync(string email = "", string userName = "", string userId = "")
        {
            if (!string.IsNullOrWhiteSpace(email))
                return await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(userName))
                return await _userManager.FindByNameAsync(userName).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(userId))
                return await _userManager.FindByIdAsync(userId).ConfigureAwait(false);

            return null;
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
            return new JwtTokenResponse(tokenString, token.ValidTo);
        }

        private async Task ProcessPasswordVerificationResult(User user, PasswordVerificationResult verificationResult)
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

        private async Task UpdateUserPasswordHashAsync(User user)
        {
            var result = await _userManager.HasPasswordAsync(user).ConfigureAwait(false);
            if (!result)
            {
                var ex = new InvalidOperationException("Error hashing again user password");
                _logger?.LogError(ex, "Rehash needed but failed", user);
            }
        }

        #endregion
    }

    public interface IIdentityService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request">DTO with user information</param>
        /// <returns>The operation result</returns>
        Task<UserSignUpResponse> CreateNewUserAsync(UserSignUpRequest request);

        /// <summary>
        /// Create a new admin
        /// </summary>
        /// <param name="request">DTO with admin information</param>
        /// <returns>The operation result</returns>
        Task<AdminSignUpResponse> CreateNewAdminAsync(AdminSignUpRequest request);

        /// <summary>
        /// Gets all the roles
        /// </summary>
        /// <returns>Role list</returns>
        Task<IList<RoleGetResponse>> GetRolesAsync();

        /// <summary>
        /// Add a new identity role
        /// </summary>
        /// <param name="request">Role to add</param>
        /// <returns>An asynchronous result</returns>
        Task CreateNewRoleAsync(SimpleRoleNameRequest request);

        /// <summary>
        /// Delete an identity role
        /// </summary>
        /// <param name="request">Role to delete</param>
        /// <returns>An asynchronous result</returns>
        Task DeleteNewRoleAsync(SimpleRoleNameRequest request);

        /// <summary>
        /// Rename an identity role name
        /// </summary>
        /// <param name="request">New role name</param>
        /// <returns>An asynchronous result</returns>
        Task UpdateRoleNameAsync(RoleUpdateNameRequest request);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="pageSize">How many users must be returned per page</param>
        /// <param name="pageIndex">Page index</param>
        /// <returns>Returns list of users</returns>
        Task<IList<UserGetAllResponse>> GetAllUsersAsync(int pageSize, int pageIndex);

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="request">User id</param>
        /// <returns>Returns user details</returns>
        Task<UserGetResponse> GetUserByIdAsync(UserGetRequest request);

        /// <summary>
        /// Update user properties
        /// </summary>
        /// <param name="request">New properties</param>
        /// <returns></returns>
        Task UpdateUserAsync(UserUpdateRequest request);

        /// <summary>
        /// Soft delete a user
        /// </summary>
        /// <param name="request">User id</param>
        /// <returns>The operation result</returns>
        Task SoftDeleteUserAsync(UserGetRequest request);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task DeleteUserAsync(UserGetRequest request);

        /// <summary>
        /// Login the given user
        /// </summary>
        /// <param name="request">DTO with user information</param>
        /// <returns>The operation result. It contains the token if successful</returns>
        Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request);
        /// <summary>
        /// Verify the user email through the email link
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="confirmationCode"></param>
        /// <returns>The operation result</returns>
        Task<VerifyUserEmailResponse> VerifyAccountEmailAsync(string userId, string confirmationCode);

        /// <summary>
        /// Allow user to change its personal password
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="currentPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Returns true if the password is changed, otherwise false</returns>
        Task<bool> ChangeUserPasswordAsync(string email, string currentPassword, string newPassword);

        Task GenerateUserPasswordTokenAsync(string email, string username);

        Task<VerifyUserEmailResponse> ResetUserPasswordAsync(string userId, string token, string newPassword);
    }
}
