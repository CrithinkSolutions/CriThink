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
            builder.Ignore(dn => dn.DomainEvents);

            var languageItalian = DebunkingNewsLanguage.CreateSeed(
                Guid.Parse("b5165f46-b82e-46c3-9b98-e5a37a10276f"),
                EntityConstants.LanguageCodeIt,
                "Italian");

            var languageEnglish = DebunkingNewsLanguage.CreateSeed(
                Guid.Parse("cea0eeea-ec03-483e-be0f-e2f1af7669d8"),
                EntityConstants.LanguageCodeEn,
                "English");

            builder.HasKey(dnl => dnl.Id);
            builder.Property(dn => dn.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dnl => dnl.Name)
                .IsRequired();

            builder.Property(dnl => dnl.Code)
                .IsRequired();

            builder.HasIndex(dnl => dnl.Code)
                .IsUnique();

            builder.HasData(languageItalian, languageEnglish);
        }
    }
}
