using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Entities
{
    public class UnknownNewsSource : Entity<Guid>
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

        public string Uri { get; private set; }

        public DateTimeOffset FirstRequestedAt { get; private set; }

        public DateTimeOffset? IdentifiedAt { get; private set; }

        public int RequestCount { get; private set; }

        public NewsSourceAuthenticity Authenticity { get; private set; }

        public ICollection<UnknownNewsSourceNotificationRequest> NotificationQueue { get; private set; }

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

        #endregion

        public void UpdateIdentifiedAt(DateTime utcNow)
        {
            IdentifiedAt = utcNow;
        }

        public void UpdateAuthenticity(NewsSourceAuthenticity classification)
        {
            Authenticity = classification;
        }
    }
}
