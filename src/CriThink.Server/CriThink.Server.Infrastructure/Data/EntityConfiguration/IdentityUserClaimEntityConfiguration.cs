using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class IdentityUserClaimEntityConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            builder.ToTable("aspnet_user_claims");
            builder.HasData(new List<IdentityUserClaim<Guid>>
            {
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                    ClaimType = ClaimTypes.NameIdentifier,
                    ClaimValue = "f62fc754-e296-4aca-0a3f-08d88b1daff7",
                    Id = 1
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                    ClaimType = ClaimTypes.Email,
                    ClaimValue = "service@crithink.com",
                    Id = 2
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                    ClaimType = ClaimTypes.Name,
                    ClaimValue = "service",
                    Id = 3
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                    ClaimType = ClaimTypes.Role,
                    ClaimValue = "Admin",
                    Id = 4
                }
            });
        }
    }
}
