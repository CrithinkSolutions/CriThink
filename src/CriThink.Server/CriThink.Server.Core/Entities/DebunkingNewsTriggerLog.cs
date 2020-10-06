using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    public class DebunkingNewsTriggerLog : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool IsSuccessful { get; set; }

        public string TimeStamp { get; set; }
    }
}
