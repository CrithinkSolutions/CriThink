using System;
using System.Diagnostics;
using CriThink.Server.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.Ignore(property => property.TwoFactorEnabled);
            builder.Ignore(property => property.PhoneNumberConfirmed);

            var user = new User
            {
                Id = Guid.Parse("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                SecurityStamp = "XV7NZ5BSN7ASJO6OMO3WT2L75Y2TI6VD",
                Email = "service@crithink.com",
                EmailConfirmed = true,
                UserName = "service",
                NormalizedEmail = "SERVICE@CRITHINK.COM",
                NormalizedUserName = "SERVICE",
                ConcurrencyStamp = "c31844c9-d81b-4c66-991c-a60b0ba36f76",
            };

            PassGenerate(user);

            builder.HasData(user);
        }

        [Conditional("DEBUG")]
        private static void PassGenerate(User user)
        {
            var passHash = new PasswordHasher<User>();
            var psw = passHash.HashPassword(user, "king2Pac!");
            user.PasswordHash = psw;
        }
    }
}
