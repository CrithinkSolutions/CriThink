using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CriThink.Server.Core.Commands;

namespace CriThink.Server.Core.Entities
{
    public class UserSearch : ICriThinkIdentity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string NewsLink { get; set; }

        [Required]
        public NewsSourceAuthenticity Authenticity { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        #region Foreign Key

        public Guid UserId { get; set; }

        [Required]
        public User User { get; set; }

        #endregion
    }
}
