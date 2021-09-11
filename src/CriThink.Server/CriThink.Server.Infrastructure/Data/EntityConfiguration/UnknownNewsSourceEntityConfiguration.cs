using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UnknownNewsSourceEntityConfiguration : IEntityTypeConfiguration<UnknownNewsSource>
    {
        public void Configure(EntityTypeBuilder<UnknownNewsSource> builder)
        {
            builder.ToTable("unknown_news_sources");

            builder.Ignore(uns => uns.DomainEvents);

            builder.HasKey(uns => uns.Id);
            builder.Property(uns => uns.Id)
                .ValueGeneratedOnAdd();

            builder.Property(uns => uns.Uri)
                .IsRequired();

            builder.HasIndex(us => us.Uri)
                .IsUnique();

            builder.Property(uns => uns.FirstRequestedAt)
                .IsRequired();

            builder.Property(uns => uns.IdentifiedAt);

            builder.Property(uns => uns.RequestCount)
                .IsRequired();

            builder.Property(uns => uns.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));


        }
    }
}
