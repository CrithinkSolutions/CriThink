using System;
using System.Diagnostics.CodeAnalysis;
using CriThink.Server.Core.DomainEvents;

namespace CriThink.Server.Core.Entities
{
    public class UnknownNewsSourceNotificationRequest : Entity<Guid>, IAggregateRoot
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

        #region Relationships

        public virtual Guid UnknownNewsSourceId { get; private set; }

        public virtual UnknownNewsSource UnknownNewsSource { get; private set; }

        #endregion

        #region Create

        public static UnknownNewsSourceNotificationRequest Create(
            string userEmail,
            UnknownNewsSource unknownNewsSource)
        {
            return new UnknownNewsSourceNotificationRequest(
                userEmail,
                unknownNewsSource);
        }

        public void SendNotification()
        {
            AddDomainEvent(
                new NewsSourceIdentifiedDomainEvent(
                    Email,
                    UnknownNewsSource,
                    RequestedAt));
        }

        #endregion
    }
}
