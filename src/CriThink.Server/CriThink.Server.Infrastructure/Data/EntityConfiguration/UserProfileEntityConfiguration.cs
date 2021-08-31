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
            builder.Ignore(dn => dn.DomainEvents);

            builder.HasKey(up => up.Id);
            builder.Property(up => up.Id)
                .ValueGeneratedOnAdd();

            builder.Property(up => up.GivenName);
            builder.Property(up => up.FamilyName);
            builder.Property(up => up.Description);
            builder.Property(up => up.Gender);
            builder.Property(up => up.AvatarPath);
            builder.Property(up => up.Country);
            builder.Property(up => up.Telegram);
            builder.Property(up => up.Skype);
            builder.Property(up => up.Twitter);
            builder.Property(up => up.Instagram);
            builder.Property(up => up.Facebook);
            builder.Property(up => up.Snapchat);
            builder.Property(up => up.Youtube);
            builder.Property(up => up.Blog);
            builder.Property(up => up.UserId);
            builder.Property(up => up.User)
                .IsRequired();

            builder
                .Property(property => property.RegisteredOn)
                .HasColumnType(DataType.Date.ToString())
                .IsRequired();

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
            var profile = UserProfile.Create(
                Guid.Parse("CB825A64-9CDB-48E7-8BB0-45D5BED6EEE2"),
                DateTime.Parse("2021-01-01"),
                DateTime.Parse("2021-01-01"),
                "This is the default account",
                Guid.Parse(UserEntityConfiguration.UserId));

            builder.HasData(profile);
        }
    }
}