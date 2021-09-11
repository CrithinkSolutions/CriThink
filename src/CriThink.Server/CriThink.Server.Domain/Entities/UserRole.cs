using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace CriThink.Server.Domain.Entities
{
    /// <summary>
    /// Entity to customize the AspNetCore.IdentityRole class
    /// </summary>
    public class UserRole : IdentityRole<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected UserRole()
        { }

        private UserRole(
            Guid id,
            string roleName,
            string concurrencyStamp)
        {
            Id = id;
            Name = roleName;
            NormalizedName = roleName?.ToUpperInvariant();
            ConcurrencyStamp = concurrencyStamp;
        }

        #region Create

        public static UserRole Create(
            Guid id,
            string roleName,
            string concurrencyStamp)
        {
            return new UserRole(
                id,
                roleName,
                concurrencyStamp);
        }

        #endregion
    }
}
