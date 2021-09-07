using System;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class NewsSourcePostQuestion : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected NewsSourcePostQuestion()
        { }

        private NewsSourcePostQuestion(
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

        public static NewsSourcePostQuestion Create(
            Guid id,
            string question,
            decimal ratio)
        {
            return new NewsSourcePostQuestion(
                id,
                question,
                ratio);
        }

        #endregion
    }
}
