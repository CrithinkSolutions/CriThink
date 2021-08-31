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

            var fakeNews = NewsSourceCategory.Create(
                Guid.Parse("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"),
                "Sources in this category show extreme bias, poor or no sourcing to credible information, a complete lack of transparency and/or publish fake news for profit or ideologically influence the audience. These sources may be very untrustworthy and should be fact checked.",
                NewsSourceAuthenticity.FakeNews
            );
            var satirical = NewsSourceCategory.Create(

                Guid.Parse("762f6747-e0de-4b3b-80b1-46d599ce02df"),
                "These sources exclusively use humor, irony, exaggeration, or ridicule to expose and criticize people’s stupidity or vices, particularly in the context of contemporary politics and other topical issues. Primarily these sources are clear that they are satire and do not attempt to deceive.",
                NewsSourceAuthenticity.Satirical
            );
            var reliable = NewsSourceCategory.Create(
                Guid.Parse("6254c650-76f0-4bba-ba5e-01218891f729"),
                "These sources have minimal bias and use very few loaded words (wording that attempts to influence an audience by using appeal to emotion or stereotypes).  The reporting is factual and usually sourced.",
                NewsSourceAuthenticity.Reliable
            );
            var conspiracist = NewsSourceCategory.Create(
                Guid.Parse("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"),
                "Sources in the Conspiracy category may publish unverifiable information that is not always supported by evidence. Actually, they usually publish Conspiracy theories consisting in explanations for an event or situation that invoke a conspiracy by sinister and powerful groups, often political in motivation. These sources may be untrustworthy for credible/verifiable information, therefore fact checking and further investigation is recommended.",
                NewsSourceAuthenticity.Conspiracist
            );
            var suspicious = NewsSourceCategory.Create(
                Guid.Parse("3cc36977-5115-45ea-88e9-7c86f19b6cd6"),
                "These sources may publish a mix of non-factual and factual information which is misrepresented and/or reported with bias (e.g. political leaning bias). Therefore, information coming from these sources should be double-checked considering reliable sources of news.",
                NewsSourceAuthenticity.Suspicious
            );
            var socialMedia = NewsSourceCategory.Create(
                Guid.Parse("d66bf55d-d30d-448f-be69-d2e0cebdd26a"),
                "Social media platforms (such as Facebook, Twitter, Reddit and so on) cannot be classified strictly as sources of information. Rather these platforms are just mediums through which information can be shared. In these cases, verifying the reliability of the user/page sharing the information is recommended.",
                NewsSourceAuthenticity.SocialMedia
            );

            builder.HasData(fakeNews, satirical, reliable, conspiracist, suspicious, socialMedia);
        }
    }
}
