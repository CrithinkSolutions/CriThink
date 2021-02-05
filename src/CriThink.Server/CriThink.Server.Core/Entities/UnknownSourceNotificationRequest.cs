using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    public class UnknownSourceNotificationRequest : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Email { get; set; }

        public DateTime RequestedAt { get; set; }

        public Guid NewsSourceId { get; set; }
    }
}
