using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class DebunkingNewsPublisher : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected DebunkingNewsPublisher()
        { }

        private DebunkingNewsPublisher(
            Guid id,
            string name,
            string link,
            string description,
            string opinion,
            string facebookPage,
            string instagramProfile,
            string twitterProfile,
            Guid countryId,
            Guid languageId)
        {
            Id = id;
            Name = name;
            Link = link;
            Description = description;
            Opinion = opinion;
            FacebookPage = facebookPage;
            InstagramProfile = instagramProfile;
            TwitterProfile = twitterProfile;
            Country = DebunkingNewsCountry.Create(countryId);
            Language = DebunkingNewsLanguage.Create(languageId);
        }

        public string Name { get; private set; }

        public string Link { get; private set; }

        public string Description { get; private set; }

        public string Opinion { get; private set; }

        public string FacebookPage { get; private set; }

        public string InstagramProfile { get; private set; }

        public string TwitterProfile { get; private set; }

        #region Foreign Keys

        public virtual DebunkingNewsLanguage Language { get; private set; }

        public virtual DebunkingNewsCountry Country { get; private set; }

        public static DebunkingNewsPublisher Create(
            Guid id,
            string name,
            string link,
            string description,
            string opinion,
            string facebook,
            string instagram,
            string twitter,
            Guid countryId,
            Guid languageId)
        {
            return new DebunkingNewsPublisher(
                id,
                name,
                link,
                description,
                opinion,
                facebook,
                instagram,
                twitter,
                countryId,
                languageId);
        }

        public virtual ICollection<DebunkingNews> DebunkingNews { get; private set; }

        #endregion
    }
}
