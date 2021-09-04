using System;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class ArticleQuestion : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected ArticleQuestion()
        { }

        private ArticleQuestion(
            Guid id,
            string question,
            decimal ratio)
        {
            Id = id;
            Question = question;
            Category = QuestionCategory.General;
            Ratio = ratio;
        }

        public string Question { get; private set; }

        public QuestionCategory Category { get; private set; }

        public decimal Ratio { get; private set; }

        #region Create

        public static ArticleQuestion Create(
            Guid id,
            string question,
            decimal ratio)
        {
            return new ArticleQuestion(
                id,
                question,
                ratio);
        }

        #endregion
    }
}
