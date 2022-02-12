using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class SearchedNewsEntityConfiguration : IEntityTypeConfiguration<SearchedNews>
    {
        internal static string SequenceName = $"sequence_{nameof(SearchedNews).ToLowerInvariant()}";

        public void Configure(EntityTypeBuilder<SearchedNews> builder)
        {
            builder.ToTable("searched_news");

            builder.Ignore(dn => dn.DomainEvents);

            builder.HasKey(up => up.Id);
            builder.Property(up => up.Id)
                .UseHiLo(SequenceName);

            builder.Property(up => up.Link)
                .IsRequired();

            builder.Property(up => up.Keywords);

            builder.Property(up => up.Title);

            builder.Property(up => up.FavIcon);

            builder.Property(up => up.Rate);

            builder.Property(us => us.Authenticity)
                .IsRequired()
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));

            builder.HasIndex(up => up.Link);
        }
    }
}
