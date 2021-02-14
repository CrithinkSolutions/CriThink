using System;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsLanguageEntityConfiguration : IEntityTypeConfiguration<DebunkingNewsLanguage>
    {
        public void Configure(EntityTypeBuilder<DebunkingNewsLanguage> builder)
        {
            var languageItalian = new DebunkingNewsLanguage
            {
                Id = Guid.Parse("b5165f46-b82e-46c3-9b98-e5a37a10276f"),
                Code = EntityConstants.LanguageCodeIt,
                Name = "Italian",
            };
            var languageEnglish = new DebunkingNewsLanguage
            {
                Id = Guid.Parse("cea0eeea-ec03-483e-be0f-e2f1af7669d8"),
                Code = EntityConstants.LanguageCodeEn,
                Name = "English",
            };

            builder.HasIndex(l => l.Code).IsUnique();
            builder.HasData(languageItalian, languageEnglish);
        }
    }
}
