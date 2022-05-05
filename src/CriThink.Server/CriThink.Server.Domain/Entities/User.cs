using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CriThink.Server.Domain.Constants;
using CriThink.Server.Domain.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Domain.Entities
{
    /// <summary>
    /// Database entity representing a user. Implement the AspNetCoreIdentity framework
    /// </summary>
    public class User : IdentityUser<Guid>, IAggregateRoot
    {
        private readonly List<RefreshToken> _refreshTokens = new();
        private readonly List<UserSearch> _searches = new();

        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected User()
        { }

        private User(string username, string email)
        {
            UserName = username;
            Email = email;
            Id = Guid.NewGuid();
        }

        public bool IsDeleted => _deletionScheduledOn is not null;

        private DateTimeOffset? _deletionScheduledOn;
        public DateTimeOffset? DeletionScheduledOn => _deletionScheduledOn;

        private DateTimeOffset? _deletionRequestedOn;
        public DateTimeOffset? DeletionRequestedOn => _deletionRequestedOn;

        #region Relationships

        public virtual UserProfile Profile { get; private set; }

        public virtual IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

        public virtual IReadOnlyCollection<UserSearch> Searches => _searches;

        #endregion

        #region Create

        public static User Create(string username, string email)
        {
            var user = new User(username, email);
            user.Profile = UserProfile.Create();
            return user;
        }

        public static User CreateSeed(
            string concurrencyStamp,
            Guid userId,
            string username,
            string email,
            bool emailConfirmed,
            string passwordHash,
            string securityStamp)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            var user = new User(username, email)
            {
                Id = userId,
                ConcurrencyStamp = concurrencyStamp,
                EmailConfirmed = emailConfirmed,
                PasswordHash = passwordHash,
                SecurityStamp = securityStamp,
                NormalizedEmail = email.ToUpperInvariant(),
                NormalizedUserName = username.ToUpperInvariant(),
            };

            return user;
        }

        #endregion

        public void UpdateFamilyName(string familyName)
        {
            Profile.UpdateFamilyName(familyName);
        }

        public void UpdateDescription(string description)
        {
            Profile.UpdateDescription(description);
        }

        public void UpdateTelegram(string telegram)
        {
            Profile.UpdateTelegram(telegram);
        }

        public void UpdateInstagram(string instagram)
        {
            Profile.UpdateInstagram(instagram);
        }

        public void UpdateSnapchat(string snapchat)
        {
            Profile.UpdateSnapchat(snapchat);
        }

        public void UpdateDateOfBirth(DateTime? dateOfBirth)
        {
            Profile.UpdateDateOfBirth(dateOfBirth);
        }

        public void UpdateBlog(string blog)
        {
            Profile.UpdateBlog(blog);
        }

        public void UpdateYoutube(string youtube)
        {
            Profile.UpdateYoutube(youtube);
        }

        public void UpdateFacebook(string facebook)
        {
            Profile.UpdateFacebook(facebook);
        }

        public void UpdateTwitter(string twitter)
        {
            Profile.UpdateTwitter(twitter);
        }

        public void UpdateSkype(string skype)
        {
            Profile.UpdateSkype(skype);
        }

        public void UpdateCountry(string country)
        {
            Profile.UpdateCountry(country);
        }

        public void UpdateGender(Gender? gender)
        {
            Profile.UpdateGender(gender);
        }

        public void UpdateGivenName(string givenName)
        {
            Profile.UpdateGivenName(givenName);
        }

        public void AddSearch(
            UserSearch userSearch)
        {
            _searches.Add(userSearch);
        }

        /// <summary>
        /// Returns true if the user has active
        /// refresh tokens
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        /// <summary>
        /// Adds a refresh token for this user
        /// </summary>
        /// <param name="token"></param>
        /// <param name="remoteIpAddress"></param>
        /// <param name="timeFromNow"></param>
        public void AddRefreshToken(string token, string remoteIpAddress, TimeSpan timeFromNow)
        {
            var refreshToken = RefreshToken.Create(token, timeFromNow, this, remoteIpAddress);
            _refreshTokens.Add(refreshToken);
        }

        /// <summary>
        /// Removes a refresh token from this user
        /// </summary>
        /// <param name="refreshToken"></param>
        public void RemoveRefreshToken(string refreshToken)
        {
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        }

        /// <summary>
        /// Logically delete this user. Schedules deletion
        /// 3 months from now
        /// </summary>
        public void Delete()
        {
            if (_deletionRequestedOn is not null)
                return;

            _deletionRequestedOn = DateTimeOffset.UtcNow;
            _deletionScheduledOn = DateTimeOffset.UtcNow.AddMonths(3);
        }

        /// <summary>
        /// Fully restore this user from a previously
        /// deletion request
        /// </summary>
        public void CancelScheduledDeletion()
        {
            _deletionRequestedOn = null;
            _deletionScheduledOn = null;
            EmailConfirmed = false;
        }

        public async Task UpdateUserProfileAvatarAsync(
            IFileService fileService,
            IFormFile formFile)
        {
            if (fileService is null)
                throw new ArgumentNullException(nameof(fileService));

            if (formFile is null)
                throw new ArgumentNullException(nameof(formFile));

            var uri = await fileService.SaveFileAsync(
                formFile,
                true,
                Id,
                ProfileConstants.AvatarFileName);

            Profile.UpdateUserAvatar(uri.AbsolutePath);
        }

        public async Task UpdateUserProfileAvatarAsync(
            IFileService fileService,
            byte[] bytes)
        {
            if (fileService is null)
                throw new ArgumentNullException(nameof(fileService));

            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            var uri = await fileService.SaveFileAsync(
                bytes,
                true,
                Id,
                ProfileConstants.AvatarFileName);

            Profile.UpdateUserAvatar(uri.AbsolutePath);
        }

        public async Task DeleteUserUserProfileAvatarAsync(
            IFileService fileService)
        {
            if (fileService is null)
                throw new ArgumentNullException(nameof(fileService));

            await fileService.DeleteFileAsync(
                Id,
                ProfileConstants.AvatarFileName);
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
        }
    }
}
