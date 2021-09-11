using System;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsPublisherEntityConfiguration : IEntityTypeConfiguration<DebunkingNewsPublisher>
    {
        public void Configure(EntityTypeBuilder<DebunkingNewsPublisher> builder)
        {
            builder.ToTable("debunking_news_publishers");

            builder.Ignore(dn => dn.DomainEvents);

            builder.HasKey(dn => dn.Id);
            builder.Property(dn => dn.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dn => dn.Name)
                .IsRequired();
            builder.HasIndex(dn => dn.Name)
                .IsUnique();

            builder.Property(dn => dn.Link)
                .IsRequired();

            builder.Property(dn => dn.Description);
            builder.Property(dn => dn.Opinion);
            builder.Property(dn => dn.FacebookPage);
            builder.Property(dn => dn.InstagramProfile);
            builder.Property(dn => dn.TwitterProfile);

            builder
                .HasOne(p => p.Country)
                .WithMany(c => c.Publishers)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Language)
                .WithMany(l => l.Publishers)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(new[]
            {
                DebunkingNewsPublisher.CreateSeed(
                    Guid.Parse("ec22b726-c503-4bfc-ae33-eb6729b22bef"),
                    EntityConstants.OpenOnline,
                    EntityConstants.OpenOnlineLink,
                    EntityConstants.OpenOnlineDescription,
                    EntityConstants.OpenOnlineOpinion,
                    EntityConstants.OpenOnlineFacebook,
                    EntityConstants.OpenOnlineInstagram,
                    EntityConstants.OpenOnlineTwitter,
                    Guid.Parse("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                    Guid.Parse("b5165f46-b82e-46c3-9b98-e5a37a10276f")
                ),
                DebunkingNewsPublisher.CreateSeed(
                    Guid.Parse("3181faf4-45e2-4a91-8340-8ed9598513c8"),
                    EntityConstants.Channel4,
                    EntityConstants.Channel4Link,
                    EntityConstants.Channel4Description,
                    EntityConstants.Channel4Opinion,
                    EntityConstants.Channel4Facebook,
                    EntityConstants.Channel4Instagram,
                    EntityConstants.Channel4Twitter,
                    Guid.Parse("812361b1-d1c3-4315-b601-4e060364a1d6"),
                    Guid.Parse("cea0eeea-ec03-483e-be0f-e2f1af7669d8")
                ),
                DebunkingNewsPublisher.CreateSeed(
                    Guid.Parse("511199ed-595c-4830-a40b-bcb58ca7bbb2"),
                    EntityConstants.FullFact,
                    EntityConstants.FullFactLink,
                    EntityConstants.FullFactDescription,
                    EntityConstants.FullFactOpinion,
                    EntityConstants.FullFactFacebook,
                    EntityConstants.FullFactInstagram,
                    EntityConstants.FullFactTwitter,
                    Guid.Parse("812361b1-d1c3-4315-b601-4e060364a1d6"),
                    Guid.Parse("cea0eeea-ec03-483e-be0f-e2f1af7669d8")
                ),
                DebunkingNewsPublisher.CreateSeed(
                    Guid.Parse("80aa1eaf-d64b-46eb-a438-503b716f9c2a"),
                    EntityConstants.FactaNews,
                    EntityConstants.FactaNewsLink,
                    EntityConstants.FactaNewsDescription,
                    EntityConstants.FactaNewsOpinion,
                    EntityConstants.FactaNewsFacebook,
                    EntityConstants.FactaNewsInstagram,
                    EntityConstants.FactaNewsTwitter,
                    Guid.Parse("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                    Guid.Parse("b5165f46-b82e-46c3-9b98-e5a37a10276f")
                )
            });
        }
    }
}
