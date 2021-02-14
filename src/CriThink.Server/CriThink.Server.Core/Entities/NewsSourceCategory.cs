using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Entities
{
    public class NewsSourceCategory : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public NewsSourceAuthenticity Authenticity { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
    }
}
