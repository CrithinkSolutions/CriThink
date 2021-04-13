using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class IdentityUserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        private const string AdminRoleId = "EC1405D9-5E55-401A-B469-37A44ECD211F";
        private const string ServiceUserId = "f62fc754-e296-4aca-0a3f-08d88b1daff7";

        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.ToTable("aspnet_user_roles");

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.HasData(new IdentityUserRole<Guid>
            {
                RoleId = Guid.Parse(AdminRoleId),
                UserId = Guid.Parse(ServiceUserId),
            });
        }
    }
}
