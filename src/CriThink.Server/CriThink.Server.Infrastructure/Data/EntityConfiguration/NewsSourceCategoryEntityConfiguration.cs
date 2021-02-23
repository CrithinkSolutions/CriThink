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
                Description = "Sources in this category show extreme bias, poor or no sourcing to credible information, a complete lack of transparency and/or publish fake news for profit or ideologically influence the audience. These sources may be very untrustworthy and should be fact checked.",
                Authenticity = NewsSourceAuthenticity.FakeNews,
            };
            var satirical = new NewsSourceCategory
            {
                Id = Guid.Parse("762f6747-e0de-4b3b-80b1-46d599ce02df"),
                Description = "These sources exclusively use humor, irony, exaggeration, or ridicule to expose and criticize people’s stupidity or vices, particularly in the context of contemporary politics and other topical issues. Primarily these sources are clear that they are satire and do not attempt to deceive.",
                Authenticity = NewsSourceAuthenticity.Satirical,
            };
            var reliable = new NewsSourceCategory
            {
                Id = Guid.Parse("6254c650-76f0-4bba-ba5e-01218891f729"),
                Description = "These sources have minimal bias and use very few loaded words (wording that attempts to influence an audience by using appeal to emotion or stereotypes).  The reporting is factual and usually sourced.",
                Authenticity = NewsSourceAuthenticity.Reliable,
            };
            var conspiracist = new NewsSourceCategory
            {
                Id = Guid.Parse("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"),
                Description = "Sources in the Conspiracy category may publish unverifiable information that is not always supported by evidence. Actually, they usually publish Conspiracy theories consisting in explanations for an event or situation that invoke a conspiracy by sinister and powerful groups, often political in motivation. These sources may be untrustworthy for credible/verifiable information, therefore fact checking and further investigation is recommended.",
                Authenticity = NewsSourceAuthenticity.Conspiracist,
            };

            builder.HasData(fakeNews, satirical, reliable, conspiracist);
        }
    }
}
