using System;
using CriThink.Server.Core.Entities;
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
                "Admin",
                "15b1b12c-4dff-413e-81d5-7c9423f25c35");

            builder.HasData(adminRole);
        }
    }
}
