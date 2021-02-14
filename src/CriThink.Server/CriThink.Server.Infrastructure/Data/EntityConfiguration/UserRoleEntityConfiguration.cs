using System;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        private readonly IOptions<UserRole> _adminRole;

        public UserRoleEntityConfiguration(IOptions<UserRole> userRole)
        {
            _adminRole = userRole ?? throw new ArgumentNullException(nameof(userRole));
        }

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");

            var adminRole = _adminRole.Value;
            builder.HasData(adminRole);
        }
    }
}
