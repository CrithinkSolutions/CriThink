using System;
using System.Diagnostics.CodeAnalysis;
using CriThink.Server.Domain.DomainEvents;

namespace CriThink.Server.Domain.Entities
{
    public class UnknownNewsSourceNotification : Entity<Guid>, IAggregateRoot
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected UnknownNewsSourceNotification()
        { }

        public UnknownNewsSourceNotification(
            string email,
            UnknownNewsSource unknownNewsSource)
        {
            Email = email;
            UnknownNewsSource = unknownNewsSource;
            RequestedAt = DateTime.UtcNow;

            AddDomainEvent(
                new NewsSourceNotificationCreateDomainEvent(
                    Email,
                    UnknownNewsSource));
        }

        public string Email { get; private set; }

        public DateTimeOffset RequestedAt { get; private set; }

        #region Relationships

        public virtual Guid UnknownNewsSourceId { get; private set; }

        public virtual UnknownNewsSource UnknownNewsSource { get; private set; }

        #endregion

        #region Create

        public static UnknownNewsSourceNotification Create(
            string userEmail,
            UnknownNewsSource unknownNewsSource)
        {
            return new UnknownNewsSourceNotification(
                userEmail,
                unknownNewsSource);
        }

        public void Send()
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
