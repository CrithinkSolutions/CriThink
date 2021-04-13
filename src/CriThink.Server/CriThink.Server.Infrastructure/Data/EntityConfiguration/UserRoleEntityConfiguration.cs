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

            builder.HasData(new UserRole
            {
                Id = Guid.Parse("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "43ec0e6f-4239-4e39-892f-4110060d16fa",
            });
        }
    }
}
