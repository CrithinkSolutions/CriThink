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

        public User()
        {
            _refreshTokens = new List<RefreshToken>();
        }

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        [Required]
        public bool IsDeleted { get; set; } = false;

        public string AvatarPath { get; set; }

        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, string remoteIpAddress, double daysToExpire = 5)
        {
            _refreshTokens.Add(new RefreshToken(token, DateTime.UtcNow.AddDays(daysToExpire), this, remoteIpAddress));
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        }
    }
}
