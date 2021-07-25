using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Core.Entities
{
    /// <summary>
    /// Database entity representing a user. Implement the AspNetCoreIdentity framework
    /// </summary>
    public class User : IdentityUser<Guid>, ICriThinkIdentity
    {
        private readonly List<RefreshToken> _refreshTokens;
        private readonly List<ArticleAnswer> _articleAnswers;
        private readonly List<UserSearch> _searches;

        public User()
        {
            _refreshTokens = new List<RefreshToken>();
            _articleAnswers = new List<ArticleAnswer>();
            _searches = new List<UserSearch>();
        }

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public IReadOnlyCollection<ArticleAnswer> ArticleAnswers => _articleAnswers.AsReadOnly();

        public bool IsDeleted => _deletionScheduledOn is not null;

        private DateTimeOffset? _deletionScheduledOn;
        public DateTimeOffset? DeletionScheduledOn => _deletionScheduledOn;

        private DateTimeOffset? _deletionRequestedOn;
        public DateTimeOffset? DeletionRequestedOn => _deletionRequestedOn;

        [Required]
        public UserProfile Profile { get; set; }

        public virtual ICollection<UserSearch> Searches => _searches.AsReadOnly();

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
            _refreshTokens.Add(new RefreshToken(token, DateTime.UtcNow.Add(timeFromNow), this, remoteIpAddress));
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
        }
    }
}
