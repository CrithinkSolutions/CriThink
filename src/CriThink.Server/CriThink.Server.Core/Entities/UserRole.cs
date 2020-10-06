using System;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Core.Entities
{
    /// <summary>
    /// Entity to customize the AspNetCore.IdentityRole class
    /// </summary>
    public sealed class UserRole : IdentityRole<Guid>, ICriThinkIdentity
    {
        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="name">Role name</param>
        public UserRole(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
