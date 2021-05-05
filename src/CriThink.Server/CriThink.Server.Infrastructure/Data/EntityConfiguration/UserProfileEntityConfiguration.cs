using System;
using System.ComponentModel.DataAnnotations;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    public class UserProfileEntityConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder
                .Property(property => property.RegisteredOn)
                .HasColumnType(DataType.Date.ToString());

            builder
                .Property(property => property.DateOfBirth)
                .HasColumnType(DataType.Date.ToString());

            builder.Property(us => us.Gender)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<Gender>(stringValue));

            SeedData(builder);
        }

        private static void SeedData(EntityTypeBuilder<UserProfile> builder)
        {
            var profile = new UserProfile
            {
                Id = Guid.Parse("CB825A64-9CDB-48E7-8BB0-45D5BED6EEE2"),
                RegisteredOn = DateTime.Parse("2021-01-01"),
                DateOfBirth = DateTime.Parse("2021-01-01"),
                Description = "This is the default account",
                UserId = Guid.Parse(UserEntityConfiguration.UserId),
            };

            builder.HasData(profile);
        }
    }
}