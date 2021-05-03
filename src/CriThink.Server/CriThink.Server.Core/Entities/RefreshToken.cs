using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    public class RefreshToken : ICriThinkIdentity
    {
        /// <summary>
        /// Default constructor. Required by EF
        /// </summary>
        public RefreshToken()
        { }

        public RefreshToken(string token, DateTime expiresAt, User user, string remoteIpAddress)
        {
            Token = token;
            ExpiresAt = expiresAt;
            User = user;
            RemoteIpAddress = remoteIpAddress;
        }

        public bool Active => DateTime.UtcNow <= ExpiresAt;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Token { get; private set; }

        [Required]
        public DateTime ExpiresAt { get; private set; }

        public string RemoteIpAddress { get; private set; }

        #region Foreign Keys

        [Required]
        public User User { get; set; }

        #endregion
    }
}