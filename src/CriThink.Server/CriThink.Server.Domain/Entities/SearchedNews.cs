using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CriThink.Server.Domain.Entities
{
    public class SearchedNews : Entity<long>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected SearchedNews() { }

        private SearchedNews(
            string link,
            string keywords,
            string title,
            string favicon,
            NewsSourceAuthenticity authenticity)
        {
            Link = link;
            Keywords = keywords;
            Title = title;
            FavIcon = favicon;
            Authenticity = authenticity;
        }

        private SearchedNews(
            string link,
            NewsSourceAuthenticity authenticity)
        {
            Link = link;
            Authenticity = authenticity;
        }

        public string Link { get; private set; }

        public string Keywords { get; private set; }

        public string Title { get; private set; }

        public string FavIcon { get; private set; }

        public decimal? Rate { get; private set; }

        public NewsSourceAuthenticity Authenticity { get; private set; }

        #region Create

        public static SearchedNews Create(
            string link,
            string title,
            string favicon,
            IEnumerable<string> keywords,
            NewsSourceAuthenticity authenticity)
        {
            return new SearchedNews(
                link,
                MergeKeywords(keywords),
                title,
                favicon,
                authenticity);
        }

        public static SearchedNews CreateUnknown(
            string newsLink)
        {
            return new SearchedNews(newsLink, NewsSourceAuthenticity.Unknown);
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

        private static string MergeKeywords(IEnumerable<string> keywords) =>
            string.Join(',', keywords);
    }
}
