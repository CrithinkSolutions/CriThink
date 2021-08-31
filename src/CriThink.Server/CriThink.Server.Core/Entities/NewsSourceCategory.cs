using System;
using System.Diagnostics.CodeAnalysis;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Entities
{
    public class NewsSourceCategory : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected NewsSourceCategory()
        { }

        private NewsSourceCategory(
            Guid id,
            string description,
            NewsSourceAuthenticity authenticity)
        {
            Id = id;
            Description = description;
            Authenticity = authenticity;
        }

        public NewsSourceAuthenticity Authenticity { get; private set; }

        public string Description { get; private set; }

        #region Create

        public static NewsSourceCategory Create(
            Guid id,
            string description,
            NewsSourceAuthenticity authenticity)
        {
            return new NewsSourceCategory(
                id,
                description,
                authenticity);
        }

        #endregion
    }
}
