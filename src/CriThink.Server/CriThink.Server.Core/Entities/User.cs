using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Core.Entities
{
    /// <summary>
    /// Database entity representing a user. Implement the AspNetCoreIdentity framework
    /// </summary>
    public sealed class User : IdentityUser<Guid>, ICriThinkIdentity
    {
        [Required]
        public bool IsDeleted { get; set; } = false;

        public string AvatarPath { get; set; }
    }
}
