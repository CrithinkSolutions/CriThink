using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CriThink.Server.Core.Entities
{
    public class ArticleQuestion : ICriThinkIdentity
    {
        [ExcludeFromCodeCoverage]
        internal ArticleQuestion()
        { }

        public ArticleQuestion(Guid id, string question)
        {
            Id = id;
            Question = question;
            Category = QuestionCategory.General;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Question { get; private set; }

        public QuestionCategory Category { get; private set; }
    }
}
