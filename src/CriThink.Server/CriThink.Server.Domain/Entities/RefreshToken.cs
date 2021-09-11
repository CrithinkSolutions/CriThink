using System;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Domain.Entities
{
    public class RefreshToken : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected RefreshToken()
        { }

        private RefreshToken(
            string token,
            DateTime expiresAt,
            User user,
            string remoteIpAddress)
        {
            Token = token;
            ExpiresAt = expiresAt;
            User = user;
            RemoteIpAddress = remoteIpAddress;
        }

        public bool Active => DateTime.UtcNow <= ExpiresAt;

        public string Token { get; private set; }

        public DateTimeOffset ExpiresAt { get; private set; }

        public string RemoteIpAddress { get; private set; }

        #region Relationships

        public virtual Guid UserId { get; private set; }

        public virtual User User { get; set; }

        #endregion

        #region Create

        public static RefreshToken Create(
            string token,
            TimeSpan timeFromNow,
            User user,
            string remoteIpAddress)
        {
            return new RefreshToken(
                token,
                DateTime.UtcNow.Add(timeFromNow),
                user,
                remoteIpAddress);
        }

        #endregion
    }
}