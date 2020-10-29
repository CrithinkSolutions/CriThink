using CriThink.Server.Core.Commands;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    public class NewsSourceCategory : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public NewsSourceAuthenticity Authenticity { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }
    }
}
