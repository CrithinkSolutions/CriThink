using System;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Core.Entities
{
    /// <summary>
    /// Entity to customize the AspNetCore.IdentityRole class
    /// </summary>
    public sealed class UserRole : IdentityRole<Guid>, ICriThinkIdentity
    {
        public UserRole()
        {
            Id = Guid.NewGuid();
        }

        public UserRole(string name)
            : this()
        {
            Name = name;
        }
    }
}
