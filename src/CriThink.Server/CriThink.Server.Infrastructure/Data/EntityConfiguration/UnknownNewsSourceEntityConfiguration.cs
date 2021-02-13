using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UnknownNewsSourceEntityConfiguration : IEntityTypeConfiguration<UnknownNewsSource>
    {
        public void Configure(EntityTypeBuilder<UnknownNewsSource> builder)
        {
            builder.Property(us => us.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));

            builder.HasIndex(us => us.Uri).IsUnique();

            builder
                .HasMany(r => r.NotificationQueue)
                .WithOne(r => r.UnknownNewsSource);
        }
    }
}
