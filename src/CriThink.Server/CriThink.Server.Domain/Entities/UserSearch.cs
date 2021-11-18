using System;

namespace CriThink.Server.Domain.Entities
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
            SearchedNews searchedNews)
        {
            UserId = userId;
            SearchedNews = searchedNews;
            Timestamp = DateTimeOffset.UtcNow;
        }

        private UserSearch(
            Guid userId,
            string searchedText)
        {
            UserId = userId;
            SearchedText = searchedText;
        }

        public string SearchText => SearchedNews?.Link ?? SearchedText;

        public DateTimeOffset Timestamp { get; private set; }

        public string SearchedText { get; private set; }

        #region Relationships

        public virtual SearchedNews SearchedNews { get; private set; }

        public virtual Guid UserId { get; private set; }

        public virtual User User { get; private set; }

        #endregion

        #region Create

        public static UserSearch CreateNewsSearch(
            Guid userId,
            SearchedNews searchedNews)
        {
            return new UserSearch(
                userId,
                searchedNews);
        }

        public static UserSearch CreateTextSearch(
            Guid userId,
            string searchedText)
        {
            return new UserSearch(
                userId,
                searchedText);
        }

        #endregion
    }
}
