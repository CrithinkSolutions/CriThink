using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Identity
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public Task<IdentityResult> CreateUserAsync(User user, string plainPassword = null)
        {
            return string.IsNullOrWhiteSpace(plainPassword) ?
                _userManager.CreateAsync(user) :
                _userManager.CreateAsync(user, plainPassword);
        }

        public async Task<string> GetEmailConfirmationTokenAsync(User user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user)
                .ConfigureAwait(false);

            return Base64Helper.ToBase64(code);
        }

        public Task<IdentityResult> ConfirmUserEmailAsync(User user, string encodedConfirmationCode)
        {
            var decodedCode = Base64Helper.FromBase64(encodedConfirmationCode);
            return _userManager.ConfirmEmailAsync(user, decodedCode);
        }

        public Task<IdentityResult> AddUserToRoleAsync(User user, string roleName)
        {
            return _userManager.AddToRoleAsync(user, roleName);
        }

        public Task<IdentityResult> AddClaimsToUserAsync(User user, IList<Claim> claims)
        {
            return _userManager.AddClaimsAsync(user, claims);
        }

        public Task<IList<Claim>> GetUserClaimsAsync(User user)
        {
            return _userManager.GetClaimsAsync(user);
        }

        public Task<IList<string>> GetUserRolesAsync(User user)
        {
            return _userManager.GetRolesAsync(user);
        }

        public Task<IdentityResult> RemoveUserFromRolesAsync(User user, IList<string> roles)
        {
            return _userManager.RemoveFromRolesAsync(user, roles);
        }

        public Task<IdentityResult> RemoveUserFromRoleAsync(User user, string role)
        {
            return _userManager.RemoveFromRoleAsync(user, role);
        }

        public Task<List<User>> GetAllUsersAsync(int pageSize, int pageIndex)
        {
            return _userManager.Users
                .OrderBy(u => u.UserName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize + 1)
                .ToListAsync();
        }

        public Task<IdentityResult> UpdateUserAsync(User user)
        {
            return _userManager.UpdateAsync(user);
        }

        public PasswordVerificationResult VerifyUserPassword(User user, string password)
        {
            return _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        }

        public Task<IdentityResult> ChangeUserPasswordAsync(User user, string currentPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public Task<string> GenerateUserPasswordResetTokenAsync(User user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public Task<IdentityResult> ResetUserPasswordAsync(User user, string resetToken, string password)
        {
            var decodedToken = Base64Helper.FromBase64(resetToken);
            return _userManager.ResetPasswordAsync(user, decodedToken, password);
        }

        public async Task<User> FindUserAsync(string value)
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

        public Task<bool> HasUserPasswordAsync(User user)
        {
            return _userManager.HasPasswordAsync(user);
        }

        public Task<User> FindUserByLoginAsync(string provider, string userId)
        {
            return _userManager.FindByLoginAsync(provider, userId);
        }

        public Task<IdentityResult> AddUserLoginAsync(User user, UserLoginInfo userLoginInfo)
        {
            return _userManager.AddLoginAsync(user, userLoginInfo);
        }

        public Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public Task SignInAsync(User user, bool isPersistent)
        {
            return _signInManager.SignInAsync(user, isPersistent);
        }
    }
}
