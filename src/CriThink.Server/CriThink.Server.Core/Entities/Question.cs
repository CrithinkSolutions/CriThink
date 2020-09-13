using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace CriThink.Server.Core.Entities
{
    public class Question : ICriThinkIdentity, IRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
