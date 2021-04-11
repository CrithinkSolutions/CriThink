using System;
using System.Collections.Generic;
using System.Security.Claims;
using CriThink.Server.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class IdentityUserClaimEntityConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
    {
        private readonly IOptions<User> _serviceUser;
        private readonly IOptions<UserRole> _adminRole;

        public IdentityUserClaimEntityConfiguration(IOptions<User> configuration, IOptions<UserRole> userRole)
        {
            //_serviceUser = configuration ?? throw new ArgumentNullException(nameof(configuration));
            //_adminRole = userRole ?? throw new ArgumentNullException(nameof(userRole));
        }

        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            //var serviceUser = _serviceUser.Value;
            //var adminRole = _adminRole.Value;

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
