using System;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class NewsSourceCategoryEntityConfiguration : IEntityTypeConfiguration<NewsSourceCategory>
    {
        public void Configure(EntityTypeBuilder<NewsSourceCategory> builder)
        {
            builder.ToTable("news_source_categories");

            builder.Ignore(dn => dn.DomainEvents);

            builder.HasKey(dntl => dntl.Id);
            builder.Property(dntl => dntl.Id)
                .ValueGeneratedOnAdd();

            builder.Property(nsc => nsc.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(nsc => nsc.Authenticity)
                .IsRequired()
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));

            builder.HasIndex(c => c.Authenticity)
                .IsUnique(true);

            var fakeNews = NewsSourceCategory.CreateSeed(
                Guid.Parse("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"),
                "NewsSourceFakeNews",
                NewsSourceAuthenticity.FakeNews);

            var satirical = NewsSourceCategory.CreateSeed(
                Guid.Parse("762f6747-e0de-4b3b-80b1-46d599ce02df"),
               "NewsSourceSatirical",
                NewsSourceAuthenticity.Satirical);

            var reliable = NewsSourceCategory.CreateSeed(
                Guid.Parse("6254c650-76f0-4bba-ba5e-01218891f729"),
               "NewsSourceReliable",
                NewsSourceAuthenticity.Reliable);

            var conspiracist = NewsSourceCategory.CreateSeed(
                Guid.Parse("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"),
                "NewsSourceConspiracist",
                NewsSourceAuthenticity.Conspiracist);

            var suspicious = NewsSourceCategory.CreateSeed(
                Guid.Parse("3cc36977-5115-45ea-88e9-7c86f19b6cd6"),
                "NewsSourceSuspicious",
                NewsSourceAuthenticity.Suspicious);

            var socialMedia = NewsSourceCategory.CreateSeed(
                Guid.Parse("d66bf55d-d30d-448f-be69-d2e0cebdd26a"),
                "NewsSourceSocialMedia",
                NewsSourceAuthenticity.SocialMedia);

            builder.HasData(fakeNews, satirical, reliable, conspiracist, suspicious, socialMedia);
        }
    }
}
