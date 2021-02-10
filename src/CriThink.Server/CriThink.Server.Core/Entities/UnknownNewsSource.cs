﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Entities
{
    public class UnknownNewsSource : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Uri { get; set; }

        public DateTime FirstRequestedAt { get; set; }

        public DateTime? IdentifiedAt { get; set; }

        public int RequestCount { get; set; }

        public NewsSourceAuthenticity Authenticity { get; set; }
    }
}