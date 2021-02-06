using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    public class UnknownSourceNotificationRequest : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        public DateTime RequestedAt { get; set; }

        [Required]
        public Guid UnknownNewsSourceId { get; set; }
    }
}
