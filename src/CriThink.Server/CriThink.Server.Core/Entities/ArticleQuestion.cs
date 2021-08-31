using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class ArticleQuestion : Entity<Guid>
    {
        [ExcludeFromCodeCoverage]
        internal ArticleQuestion()
        { }

        public ArticleQuestion(Guid id, string question, decimal ratio)
        {
            Id = id;
            Question = question;
            Category = QuestionCategory.General;
            Ratio = ratio;
        }

        [Required]
        public string Question { get; private set; }

        [Required]
        public QuestionCategory Category { get; private set; }

        [Required]
        public decimal Ratio { get; private set; }
    }
}
