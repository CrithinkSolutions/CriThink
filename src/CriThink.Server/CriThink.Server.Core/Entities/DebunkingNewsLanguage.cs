using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
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

        public virtual ICollection<DebunkingNewsPublisher> Publishers { get; private set; }

        #region Create

        public static DebunkingNewsLanguage Create(
            Guid id,
            string languageCode,
            string name)
        {
            return new DebunkingNewsLanguage(
                id,
                languageCode,
                name);
        }

        internal static DebunkingNewsLanguage Create(Guid languageId)
        {
            return new DebunkingNewsLanguage(languageId);
        }

        #endregion
    }
}