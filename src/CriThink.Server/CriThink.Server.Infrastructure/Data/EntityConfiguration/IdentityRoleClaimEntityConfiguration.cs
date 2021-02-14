using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class IdentityRoleClaimEntityConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            builder.ToTable("aspnet_role_claims");
        }
    }
}
