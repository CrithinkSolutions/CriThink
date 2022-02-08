using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Common.Helpers;
using CriThink.Server.Domain.Entities;
using CriThink.Server.Domain.Repositories;
using CriThink.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Identity
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly CriThinkDbContext _dbContext;

        public UserRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            CriThinkDbContext dbContext)
        {
            _userManager = userManager ??
                throw new ArgumentNullException(nameof(userManager));

            _signInManager = signInManager ??
                throw new ArgumentNullException(nameof(signInManager));

            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public Task<IdentityResult> CreateUserAsync(User user, string plainPassword = null)
        {
            return string.IsNullOrWhiteSpace(plainPassword) ?
                _userManager.CreateAsync(user) :
                _userManager.CreateAsync(user, plainPassword);
        }

        public Task<IdentityResult> AddClaimsToUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Role, RoleNames.FreeUser),
            };

            return _userManager.AddClaimsAsync(user, claims);
        }

        public Task<IdentityResult> AddClaimsToAdminUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Role, RoleNames.Admin),
            };

            return _userManager.AddClaimsAsync(user, claims);
        }

        public async Task<string> GetEmailConfirmationTokenAsync(User user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user)
                .ConfigureAwait(false);

            return Base64Helper.ToBase64(code);
        }

        public async Task<User> GetUserByIdAsync(
            Guid id,
            bool includeSearches = false,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            IQueryable<User> query = _dbContext
                .Users
                .Include(u => u.RefreshTokens);

            if (includeSearches)
                query = query.Include(u => u.Searches).ThenInclude(s => s.SearchedNews);

            return await query
                .SingleOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        }

        public async Task<User> FindUserAsync(string value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            IQueryable<User> query = _userManager
                .Users
                .Include(u => u.RefreshTokens);

            Expression<Func<User, bool>> whereClause;

            if (EmailHelper.IsEmail(value))
            {
                whereClause = (u) => u.NormalizedEmail == value.ToUpperInvariant();
            }
            else if (Guid.TryParse(value, out var guid))
            {
                whereClause = (u) => u.Id.Equals(guid);
            }
            else
            {
                whereClause = (u) => u.NormalizedUserName == value.ToUpperInvariant();
            }

            return await query
                .FirstOrDefaultAsync(whereClause, cancellationToken);
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

        public async Task<string> GenerateUserPasswordResetTokenAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Base64Helper.ToBase64(token);
        }

        public Task<IdentityResult> ResetUserPasswordAsync(User user, string resetToken, string password)
        {
            var decodedToken = Base64Helper.FromBase64(resetToken);
            return _userManager.ResetPasswordAsync(user, decodedToken, password);
        }

        public async Task<User> FindUserAsync(string value, bool includeTokens, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            IQueryable<User> query = _userManager.Users;

            if (includeTokens)
            {
                query = query.Include(u => u.RefreshTokens);
            }

            Expression<Func<User, bool>> whereClause;

            if (EmailHelper.IsEmail(value))
            {
                whereClause = (u) => u.NormalizedEmail == value.ToUpperInvariant();
            }
            else if (Guid.TryParse(value, out var guid))
            {
                whereClause = (u) => u.Id.Equals(guid);
            }
            else
            {
                whereClause = (u) => u.NormalizedUserName == value.ToUpperInvariant();
            }

            return await query
                .SingleOrDefaultAsync(whereClause, cancellationToken)
                .ConfigureAwait(false);
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

        public async Task<IList<User>> DeleteUserScheduledDeletionAsync()
        {
            const string sqlCommand = "DELETE FROM users\n" +
                                      "WHERE deletion_scheduled_on < now() AT TIME ZONE 'UTC'\n" +
                                      "RETURNING *";

            var deletedUsers = await _dbContext.Users
                .FromSqlRaw(sqlCommand)
                .AsNoTracking()
                .ToListAsync();

            await _dbContext.SaveChangesAsync();

            return deletedUsers;
        }

        public async Task<User> DeleteUserByIdAsync(Guid id)
        {
            var user = _dbContext.Users
                    .FirstOrDefault(u => u.Id == id);

            user.Delete();

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<IList<decimal>> GetSearchesRateByNewsLinkAsync(
            Guid userId,
            string newsLink)
        {
            var rates = await _dbContext
                .UserSearches
                .AsNoTracking()
                .Where(u => u.SearchedNews.Link == newsLink &&
                            u.SearchedNews.Rate != null &&
                            u.UserId != userId)
                .Select(u => u.SearchedNews.Rate.Value)
                .ToListAsync();

            return rates;
        }
    }
}
