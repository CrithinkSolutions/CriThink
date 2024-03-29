﻿using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsEntityConfiguration : IEntityTypeConfiguration<DebunkingNews>
    {
        public void Configure(EntityTypeBuilder<DebunkingNews> builder)
        {
            builder.ToTable("debunking_news");

            builder.Ignore(dn => dn.DomainEvents);

            builder.HasKey(dn => dn.Id);
            builder.Property(dn => dn.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dn => dn.Title)
                .IsRequired();

            builder.Property(dn => dn.NewsCaption)
                .HasMaxLength(500);

            builder.Property(dn => dn.Link)
                .IsRequired();

            builder.HasIndex(dn => dn.Link)
                .IsUnique();

            builder.Property(dn => dn.PublishingDate)
                .IsRequired();

            builder.Property(dn => dn.ImageLink);
            builder.Property(dn => dn.Keywords);

            builder
                .HasOne(p => p.Publisher)
                .WithMany(dn => dn.DebunkingNews)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
