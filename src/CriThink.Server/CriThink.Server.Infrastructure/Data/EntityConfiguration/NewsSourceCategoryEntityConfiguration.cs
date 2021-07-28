using System;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class NewsSourceCategoryEntityConfiguration : IEntityTypeConfiguration<NewsSourceCategory>
    {
        public void Configure(EntityTypeBuilder<NewsSourceCategory> builder)
        {
            builder.Property(nsc => nsc.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));

            builder.HasIndex(c => c.Authenticity).IsUnique(true);

            var fakeNews = new NewsSourceCategory
            {
                Id = Guid.Parse("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"),
                Description = "NewsSourceFakeNews",
                Authenticity = NewsSourceAuthenticity.FakeNews,
            };
            var satirical = new NewsSourceCategory
            {
                Id = Guid.Parse("762f6747-e0de-4b3b-80b1-46d599ce02df"),
                Description = "NewsSourceSatirical",
                Authenticity = NewsSourceAuthenticity.Satirical,
            };
            var reliable = new NewsSourceCategory
            {
                Id = Guid.Parse("6254c650-76f0-4bba-ba5e-01218891f729"),
                Description = "NewsSourceReliable",
                Authenticity = NewsSourceAuthenticity.Reliable,
            };
            var conspiracist = new NewsSourceCategory
            {
                Id = Guid.Parse("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"),
                Description = "NewsSourceConspiracist",
                Authenticity = NewsSourceAuthenticity.Conspiracist,
            };
            var suspicious = new NewsSourceCategory
            {
                Id = Guid.Parse("3cc36977-5115-45ea-88e9-7c86f19b6cd6"),
                Description = "NewsSourceSuspicious",
                Authenticity = NewsSourceAuthenticity.Suspicious
            };
            var socialMedia = new NewsSourceCategory
            {
                Id = Guid.Parse("d66bf55d-d30d-448f-be69-d2e0cebdd26a"),
                Description = "NewsSourceSocialMedia",
                Authenticity = NewsSourceAuthenticity.SocialMedia
            };

            builder.HasData(fakeNews, satirical, reliable, conspiracist, suspicious, socialMedia);
        }
    }
}
