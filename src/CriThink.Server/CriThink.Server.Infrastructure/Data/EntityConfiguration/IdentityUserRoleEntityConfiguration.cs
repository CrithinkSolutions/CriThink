using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class IdentityUserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.ToTable("aspnet_user_roles");
            builder.HasData(new IdentityUserRole<Guid>
            {
                RoleId = Guid.Parse("EC1405D9-5E55-401A-B469-37A44ECD211F"),
                UserId = Guid.Parse("f62fc754-e296-4aca-0a3f-08d88b1daff7")
            });
        }
    }
}
