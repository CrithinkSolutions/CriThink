using System;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles");

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<UserRole> builder)
        {
            var adminRole = UserRole.Create(
                Guid.Parse("EC1405D9-5E55-401A-B469-37A44ECD211F"),
                RoleNames.Admin,
                "15b1b12c-4dff-413e-81d5-7c9423f25c35");

            var freeUserRole = UserRole.Create(
                Guid.Parse("4C28EED7-A34A-4534-9C2C-5FFE86B72393"),
                RoleNames.FreeUser,
                "4E597EE6-5339-44B0-988E-258AD486BE49");

            builder.HasData(adminRole, freeUserRole);
        }
    }
}
