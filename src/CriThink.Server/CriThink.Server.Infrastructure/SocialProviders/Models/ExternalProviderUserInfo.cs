using System;
using CriThink.Server.Domain.Entities;

namespace CriThink.Server.Infrastructure.SocialProviders
{
    public class ExternalProviderUserInfo
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string UserId { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Country { get; set; }

        public byte[] ProfileAvatarBytes { get; set; }
    }
}
