using System;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Entities
{
    public class UserSearch : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        protected UserSearch()
        { }

        private UserSearch(
            Guid userId,
            string newsLink,
            NewsSourceAuthenticity authenticity)
        {
            UserId = userId;
            NewsLink = newsLink;
            Authenticity = authenticity;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public string NewsLink { get; private set; }

        public NewsSourceAuthenticity Authenticity { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        #region Relationships

        public Guid UserId { get; private set; }

        public virtual User User { get; private set; }

        #endregion

        #region Create

        internal static UserSearch Create(
            Guid userId,
            string newsLink,
            NewsSourceAuthenticity authenticity)
        {
            return new UserSearch(
                userId,
                newsLink,
                authenticity);
        }

        #endregion
    }
}
