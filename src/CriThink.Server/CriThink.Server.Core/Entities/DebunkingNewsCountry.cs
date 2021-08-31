using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class DebunkingNewsCountry : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected DebunkingNewsCountry()
        { }

        private DebunkingNewsCountry(
            Guid id,
            string name,
            string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        private DebunkingNewsCountry(Guid id)
        {
            Id = id;
        }

        public string Name { get; private set; }

        public string Code { get; private set; }

        public virtual ICollection<DebunkingNewsPublisher> Publishers { get; private set; }

        #region Create

        public static DebunkingNewsCountry Create(
            Guid id,
            string name,
            string code)
        {
            return new DebunkingNewsCountry(
                id,
                name,
                code);
        }

        internal static DebunkingNewsCountry Create(Guid countryId)
        {
            return new DebunkingNewsCountry(countryId);
        }

        #endregion
    }
}