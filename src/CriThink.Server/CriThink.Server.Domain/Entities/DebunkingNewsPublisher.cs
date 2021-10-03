using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Domain.Entities
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
            CountryId = countryId;
            //Country = DebunkingNewsCountry.Create(countryId);
            LanguageId = languageId;
            //Language = DebunkingNewsLanguage.Create(languageId);
        }

        public string Name { get; private set; }

        public string Link { get; private set; }

        public string Description { get; private set; }

        public string Opinion { get; private set; }

        public string FacebookPage { get; private set; }

        public string InstagramProfile { get; private set; }

        public string TwitterProfile { get; private set; }

        #region Relationships

        public Guid CountryId { get; private set; }

        public virtual DebunkingNewsCountry Country { get; private set; }

        public Guid LanguageId { get; private set; }

        public virtual DebunkingNewsLanguage Language { get; private set; }

        public virtual ICollection<DebunkingNews> DebunkingNews { get; private set; }

        #endregion

        #region Create

        public static DebunkingNewsPublisher CreateSeed(
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

        #endregion
    }
}
