using System;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class ArticleQuestionEntityConfiguration : IEntityTypeConfiguration<ArticleQuestion>
    {
        public void Configure(EntityTypeBuilder<ArticleQuestion> builder)
        {
            builder.Property(aq => aq.Category)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<QuestionCategory>(stringValue));

            builder.HasData(new ArticleQuestion[]
            {
                new(Guid.Parse("30C7D606-D1CF-434E-A1F3-B7B1841DC331"), "HQuestion"),
                new(Guid.Parse("12F7218C-E2DD-43CD-82E0-A9216FCF6AFF"), "EQuestion"),
                new(Guid.Parse("A05D4433-2C47-4749-A8F0-FB9A6E35A868"), "AQuestion"),
                new(Guid.Parse("8731F45C-A41F-45D9-B97D-0F181E2CCE94"), "DQuestion"),
            });
        }
    }
}
