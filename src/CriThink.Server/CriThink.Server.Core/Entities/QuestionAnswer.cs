using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace CriThink.Server.Core.Entities
{
    public class QuestionAnswer : ICriThinkIdentity, IRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public bool IsTrue { get; set; }

        #region ForeignKey

        [Required]
        public Question Question { get; set; }

        [Required]
        public DemoNews DemoNews { get; set; }

        #endregion
    }
}
