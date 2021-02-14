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
            _serviceUser = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _adminRole = userRole ?? throw new ArgumentNullException(nameof(userRole));
        }

        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
        {
            var serviceUser = _serviceUser.Value;
            var adminRole = _adminRole.Value;

            builder.ToTable("aspnet_user_claims");
            builder.HasData(new List<IdentityUserClaim<Guid>>
            {
                new IdentityUserClaim<Guid>
                {
                    UserId = serviceUser.Id,
                    ClaimType = ClaimTypes.NameIdentifier,
                    ClaimValue = serviceUser.Id.ToString(),
                    Id = 1
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = serviceUser.Id,
                    ClaimType = ClaimTypes.Email,
                    ClaimValue = serviceUser.Email,
                    Id = 2
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = serviceUser.Id,
                    ClaimType = ClaimTypes.Name,
                    ClaimValue = serviceUser.UserName,
                    Id = 3
                },
                new IdentityUserClaim<Guid>
                {
                    UserId = serviceUser.Id,
                    ClaimType = ClaimTypes.Role,
                    ClaimValue = adminRole.Name,
                    Id = 4
                }
            });
        }
    }
}
