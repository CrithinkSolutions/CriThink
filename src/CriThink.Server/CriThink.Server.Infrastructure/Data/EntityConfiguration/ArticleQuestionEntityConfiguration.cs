﻿using System;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class ArticleQuestionEntityConfiguration : IEntityTypeConfiguration<ArticleQuestion>
    {
        public void Configure(EntityTypeBuilder<ArticleQuestion> builder)
        {
            builder.Ignore(aq => aq.DomainEvents);

            builder.HasKey(aa => aa.Id);
            builder.Property(aa => aa.Id)
                .ValueGeneratedOnAdd();

            builder.Property(aq => aq.Question)
                .IsRequired();

            builder.Property(aq => aq.Ratio)
                .IsRequired();

            builder.Property(aq => aq.Category)
                .IsRequired()
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<QuestionCategory>(stringValue));

            builder.HasData(new ArticleQuestion[]
            {
                ArticleQuestion.Create(Guid.Parse("30C7D606-D1CF-434E-A1F3-B7B1841DC331"), "HQuestion", 0.3m),
                ArticleQuestion.Create(Guid.Parse("12F7218C-E2DD-43CD-82E0-A9216FCF6AFF"), "EQuestion", 0.4m),
                ArticleQuestion.Create(Guid.Parse("A05D4433-2C47-4749-A8F0-FB9A6E35A868"), "AQuestion", 0.2m),
                ArticleQuestion.Create(Guid.Parse("8731F45C-A41F-45D9-B97D-0F181E2CCE94"), "DQuestion", 0.1m),
            });
        }
    }
}
