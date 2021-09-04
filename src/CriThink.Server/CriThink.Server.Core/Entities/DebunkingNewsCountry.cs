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

        public string Name { get; private set; }

        public string Code { get; private set; }

        #region Relationships

        public virtual ICollection<DebunkingNewsPublisher> Publishers { get; private set; }

        #endregion

        #region Create

        public static DebunkingNewsCountry CreateSeed(
            Guid id,
            string name,
            string code)
        {
            return new DebunkingNewsCountry(
                id,
                name,
                code);
        }

        #endregion
    }
}