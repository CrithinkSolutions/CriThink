using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CriThink.Common.Endpoints.DTOs.NewsSource;

namespace CriThink.Server.Core.Entities
{
    public class ArticleAnswer : ICriThinkIdentity
    {
        /// <summary>
        /// Default constructor used by migrations
        /// </summary>
        internal ArticleAnswer() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rate">The rate given to this news</param>
        /// <param name="newsLink">The entire news link</param>
        /// <param name="userId">The user id</param>
        public ArticleAnswer(string newsLink, User user)
        {
            if (string.IsNullOrWhiteSpace(newsLink))
                throw new ArgumentNullException(nameof(newsLink));

            NewsLink = newsLink;
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Range(1, 1000)]
        public decimal Rate { get; private set; }

        [Required]
        [MinLength(1)]
        public string NewsLink { get; private set; }

        #region Foreign Keys

        [Required]
        public User User { get; private set; }

        #endregion

        public void CalculateUserRate(NewsSourceClassification classification, IList<ArticleQuestion> questions, IList<(Guid Id, decimal Rate)> answers)
        {
            if (questions is null)
                throw new ArgumentNullException(nameof(questions));

            if (answers is null)
                throw new ArgumentNullException(nameof(answers));

            if (questions.Count != answers.Count)
                throw new ArgumentException("The number of questions and answers is different");

            Rate = 0;

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
                    nsc == NewsSourceClassification.FakeNews ||
                    nsc == NewsSourceClassification.Conspiracist => 0.7m * 0m + 0.3m * userEvaluation,

                var nsc when
                    nsc == NewsSourceClassification.Suspicious ||
                    nsc == NewsSourceClassification.SocialMedia => 0.5m * 2m + 0.5m * userEvaluation,

                NewsSourceClassification.Satirical => 0.6m * 3m + 0.4m * userEvaluation,

                NewsSourceClassification.Reliable => 0.6m * 5m + 0.4m * userEvaluation,

                _ => throw new NotImplementedException("Unknown classification type")
            };
        }
    }
}
