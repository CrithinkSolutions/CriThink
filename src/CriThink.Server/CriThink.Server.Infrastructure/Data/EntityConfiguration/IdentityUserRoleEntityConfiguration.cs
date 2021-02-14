using System;
using CriThink.Server.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class IdentityUserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        private readonly IOptions<User> _serviceUser;
        private readonly IOptions<UserRole> _adminRole;

        public IdentityUserRoleEntityConfiguration(IOptions<User> configuration, IOptions<UserRole> userRole)
        {
            _serviceUser = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _adminRole = userRole ?? throw new ArgumentNullException(nameof(userRole));
        }

        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            var adminRole = _adminRole.Value;
            var serviceUser = _serviceUser.Value;

            builder.ToTable("aspnet_user_roles");
            builder.HasData(new IdentityUserRole<Guid>
            {
                RoleId = adminRole.Id,
                UserId = serviceUser.Id
            });
        }
    }
}
