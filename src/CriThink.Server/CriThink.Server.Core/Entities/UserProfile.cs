using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriThink.Server.Core.Entities
{
    /// <summary>
    /// Database entity representing the user information
    /// </summary>
    public class UserProfile : ICriThinkIdentity, IAggregateRoot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Description { get; set; }

        public Gender? Gender { get; set; }

        public string AvatarPath { get; set; }

        public string Country { get; set; }

        public string Telegram { get; set; }

        public string Skype { get; set; }

        public string Twitter { get; set; }

        public string Instagram { get; set; }

        public string Facebook { get; set; }

        public string Snapchat { get; set; }

        public string Youtube { get; set; }

        public string Blog { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}