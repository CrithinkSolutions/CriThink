using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CriThink.Server.Core.DomainEvents;

namespace CriThink.Server.Core.Entities
{
    public class UnknownNewsSource : Entity<Guid>, IAggregateRoot
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected UnknownNewsSource()
        { }

        private UnknownNewsSource(
            Guid id,
            string link,
            NewsSourceAuthenticity authenticity)
        {
            Id = id;
            Uri = link;
            Authenticity = authenticity;
        }

        private UnknownNewsSource(
            string link,
            DateTime requestedAt)
        {
            Uri = link;
            FirstRequestedAt = requestedAt;
            Authenticity = NewsSourceAuthenticity.Unknown;
        }

        public string Uri { get; private set; }

        public DateTimeOffset FirstRequestedAt { get; private set; }

        public DateTimeOffset? IdentifiedAt { get; private set; }

        public int RequestCount { get; private set; }

        public NewsSourceAuthenticity Authenticity { get; private set; }

        public virtual ICollection<UnknownNewsSourceNotification> NotificationQueue { get; private set; }

        #region Create

        public static UnknownNewsSource Create(
            Guid id,
            string link,
            NewsSourceAuthenticity authenticity)
        {
            return new UnknownNewsSource(
                id,
                link,
                authenticity);
        }

        public static UnknownNewsSource Create(
            string link,
            DateTime requestedAt)
        {
            return new UnknownNewsSource(link, requestedAt);
        }

        #endregion

        public void UpdateIdentifiedAt(DateTime utcNow)
        {
            IdentifiedAt = utcNow;
        }

        public void UpdateAuthenticity(NewsSourceAuthenticity classification)
        {
            Authenticity = classification;
        }

        public void MarkAsKnown(
            string domain,
            NewsSourceAuthenticity authenticity)
        {
            AddDomainEvent(
                new UpdateNewsSourceRepositoryDomainEvent(
                    domain,
                    authenticity));
        }
    }
}
