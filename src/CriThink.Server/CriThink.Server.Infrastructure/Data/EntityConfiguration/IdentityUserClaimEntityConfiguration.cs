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
        private const string ServiceId = "f62fc754-e296-4aca-0a3f-08d88b1daff7";
        private const string ServiceUsername = "service";
        private const string ServiceEmail = "service@crithink.com";
        private const string ServiceRole = "Admin";

        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            builder.ToTable("aspnet_user_claims");

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            builder.HasData(new List<IdentityUserClaim<Guid>>
            {
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse(ServiceId),
                    ClaimType = ClaimTypes.NameIdentifier,
                    ClaimValue = ServiceId,
                    Id = 1
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse(ServiceId),
                    ClaimType = ClaimTypes.Email,
                    ClaimValue = ServiceEmail,
                    Id = 2
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse(ServiceId),
                    ClaimType = ClaimTypes.Name,
                    ClaimValue = ServiceUsername,
                    Id = 3
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = Guid.Parse(ServiceId),
                    ClaimType = ClaimTypes.Role,
                    ClaimValue = ServiceRole,
                    Id = 4
                }
            });
        }
    }
}
