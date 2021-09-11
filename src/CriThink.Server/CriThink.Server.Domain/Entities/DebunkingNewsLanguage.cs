using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Domain.Entities
{
    public class DebunkingNewsLanguage : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected DebunkingNewsLanguage()
        { }

        private DebunkingNewsLanguage(
            Guid id,
            string languageCode,
            string name)
        {
            Id = id;
            Code = languageCode;
            Name = name;
        }

        private DebunkingNewsLanguage(Guid id)
        {
            Id = id;
        }

        public string Name { get; private set; }

        public string Code { get; private set; }

        #region Relationships

        public virtual ICollection<DebunkingNewsPublisher> Publishers { get; private set; }

        #endregion

        #region Create

        public static DebunkingNewsLanguage CreateSeed(
            Guid id,
            string languageCode,
            string name)
        {
            return new DebunkingNewsLanguage(
                id,
                languageCode,
                name);
        }

        #endregion
    }
}