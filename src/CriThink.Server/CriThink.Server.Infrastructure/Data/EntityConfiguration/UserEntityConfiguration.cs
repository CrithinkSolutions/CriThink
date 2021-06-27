﻿using System;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public const string UserId = "f62fc754-e296-4aca-0a3f-08d88b1daff7";

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.Ignore(property => property.TwoFactorEnabled);
            builder.Ignore(property => property.PhoneNumberConfirmed);

            builder
                .Property<DateTimeOffset?>("_deletionRequestedOn")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("deletion_requested_on");

            builder
                .Property<DateTimeOffset?>("_deletionScheduledOn")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("deletion_scheduled_on");

            builder
                .HasOne(user => user.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<User> builder)
        {
            var serviceUser = new User
            {
                ConcurrencyStamp = "c31844c9-d81b-4c66-991c-a60b0ba36f76",
                Id = Guid.Parse(UserId),
                NormalizedUserName = "SERVICE",
                UserName = "service",
                NormalizedEmail = "SERVICE@CRITHINK.COM",
                Email = "service@crithink.com",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEDw0jwJ7LHQhBe2Zo45PpE6FYSpNsPyHbXP/YD51WzHrmI0MAbwHhdZf6MytihsYzg==",
                SecurityStamp = "XV7NZ5BSN7ASJO6OMO3WT2L75Y2TI6VD",
            };

            builder.HasData(serviceUser);
        }
    }
}
