using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    public class DebunkingNewsTriggerLog : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public bool IsSuccessful { get; set; }

        [Required]
        public string TimeStamp { get; set; }

        public string FailReason { get; set; }
    }
}
