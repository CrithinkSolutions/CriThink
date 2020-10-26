using System;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Models.Identity
{
    internal class User
    {
        public User(string userId, string userEmail, string userName, JwtTokenResponse jwtToken)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            UserEmail = userEmail ?? throw new ArgumentNullException(nameof(userEmail));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            JwtToken = jwtToken ?? throw new ArgumentNullException(nameof(jwtToken));
        }

        public string UserId { get; }

        public string UserEmail { get; }

        public string UserName { get; }

        public JwtTokenResponse JwtToken { get; }
    }
}
