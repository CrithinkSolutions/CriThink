using System;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class UnknownNewsSourceNotificationRequest : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected UnknownNewsSourceNotificationRequest()
        { }

        public UnknownNewsSourceNotificationRequest(
            string email,
            UnknownNewsSource unknownNewsSource)
        {
            Email = email;
            UnknownNewsSource = unknownNewsSource;
            RequestedAt = DateTime.UtcNow;
        }

        public string Email { get; private set; }

        public DateTimeOffset RequestedAt { get; private set; }

        public UnknownNewsSource UnknownNewsSource { get; private set; }

        public static UnknownNewsSourceNotificationRequest Create(
            string userEmail,
            UnknownNewsSource unknownNewsSource)
        {
            return new UnknownNewsSourceNotificationRequest(
                userEmail,
                unknownNewsSource);
        }
    }
}
