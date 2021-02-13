using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CriThink.Server.Core.Commands;

#pragma warning disable CA2227 // Collection properties should be read only
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

        public ICollection<UnknownNewsSourceNotificationRequest> NotificationQueue { get; set; }
    }
}
