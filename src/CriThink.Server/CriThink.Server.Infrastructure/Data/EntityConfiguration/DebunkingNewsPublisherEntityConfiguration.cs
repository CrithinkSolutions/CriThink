using System;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsPublisherEntityConfiguration : IEntityTypeConfiguration<DebunkingNewsPublisher>
    {
        public void Configure(EntityTypeBuilder<DebunkingNewsPublisher> builder)
        {
            builder
                .HasMany(p => p.DebunkingNews)
                .WithOne(dn => dn.Publisher)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Language)
                .WithMany(l => l.Publishers)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Country)
                .WithMany(c => c.Publishers)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasData(new[]
            {
                new
                {
                    Id = Guid.Parse("ec22b726-c503-4bfc-ae33-eb6729b22bef"),
                    Name = EntityConstants.OpenOnline,
                    Link = EntityConstants.OpenOnlineLink,
                    Description = EntityConstants.OpenOnlineDescription,
                    Opinion = EntityConstants.OpenOnlineOpinion,
                    FacebookPage = EntityConstants.OpenOnlineFacebook,
                    InstagramProfile = EntityConstants.OpenOnlineInstagram,
                    TwitterProfile = EntityConstants.OpenOnlineTwitter,
                    CountryId = Guid.Parse("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                    LanguageId = Guid.Parse("b5165f46-b82e-46c3-9b98-e5a37a10276f"),
                }
            });
        }
    }
}
