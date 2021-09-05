using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UserSearchEntityConfiguration : IEntityTypeConfiguration<UserSearch>
    {
        public void Configure(EntityTypeBuilder<UserSearch> builder)
        {
            builder.Ignore(us => us.DomainEvents);

            builder.HasKey(us => us.Id);
            builder.Property(us => us.Id)
                .ValueGeneratedOnAdd();

            builder.Property(us => us.NewsLink)
                .IsRequired();

            builder.Property(us => us.Timestamp)
                .IsRequired();

            builder.Property(us => us.Authenticity)
                .IsRequired()
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));

            builder
                .HasOne(user => user.User)
                .WithMany(s => s.Searches)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
