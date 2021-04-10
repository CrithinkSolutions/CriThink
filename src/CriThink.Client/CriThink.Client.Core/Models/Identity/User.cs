using System;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Models.Identity
{
    public class User
    {
        public User(string userId, string userEmail, string userName, string password, string avatar, JwtTokenResponse jwtToken, ExternalLoginProvider provider)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            UserEmail = userEmail ?? throw new ArgumentNullException(nameof(userEmail));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            JwtToken = jwtToken ?? throw new ArgumentNullException(nameof(jwtToken));
            AvatarPath = avatar;
            Provider = provider;
        }

        public string UserId { get; }

        public string UserEmail { get; }

        public string UserName { get; }

        public string Password { get; }

        public JwtTokenResponse JwtToken { get; }

        public ExternalLoginProvider Provider { get; }

        public string AvatarPath { get; }
    }
}
