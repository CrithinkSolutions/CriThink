using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            string newsLink,
            NewsSourceAuthenticity authenticity)
        {
            UserId = userId;
            NewsLink = newsLink;
            Authenticity = authenticity;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public string NewsLink { get; private set; }

        public NewsSourceAuthenticity Authenticity { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }

        [Range(1, 1000)] // not available as Fluent api
        public decimal? Rate { get; private set; }

        #region Relationships

        public virtual Guid UserId { get; private set; }

        public virtual User User { get; private set; }

        #endregion

        #region Create

        internal static UserSearch Create(
            Guid userId,
            string newsLink,
            NewsSourceAuthenticity authenticity)
        {
            return new UserSearch(
                userId,
                newsLink,
                authenticity);
        }

        #endregion

        public void CalculateUserRate(
            NewsSourceAuthenticity classification,
            IList<NewsSourcePostQuestion> questions,
            IList<(Guid Id, decimal Rate)> answers)
        {
            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            if (answers is null)
                throw new ArgumentNullException(nameof(answers));

            if (questions.Count != answers.Count)
                throw new ArgumentException("The number of questions and answers is different");

            decimal userEvaluation = 0;
            foreach (var question in questions)
            {
                var answer = answers.FirstOrDefault(a => a.Id == question.Id);
                if (answer.Id == Guid.Empty)
                    throw new ArgumentException("The answers are different than the questions");

                userEvaluation += answer.Rate * question.Ratio;
            }

            Rate = classification switch
            {
                var nsc when
                    nsc == NewsSourceAuthenticity.FakeNews ||
                    nsc == NewsSourceAuthenticity.Conspiracist => 0.7m * 0m + 0.3m * userEvaluation,

                var nsc when
                    nsc == NewsSourceAuthenticity.Suspicious ||
                    nsc == NewsSourceAuthenticity.SocialMedia => 0.5m * 2m + 0.5m * userEvaluation,

                NewsSourceAuthenticity.Satirical => 0.6m * 3m + 0.4m * userEvaluation,

                NewsSourceAuthenticity.Reliable => 0.6m * 5m + 0.4m * userEvaluation,

                _ => throw new NotImplementedException("Unknown classification type")
            };
        }
    }
}
