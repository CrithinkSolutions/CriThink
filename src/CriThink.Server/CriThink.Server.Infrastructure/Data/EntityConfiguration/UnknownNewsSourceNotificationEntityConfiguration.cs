using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UnknownNewsSourceNotificationEntityConfiguration : IEntityTypeConfiguration<UnknownNewsSourceNotification>
    {
        public void Configure(EntityTypeBuilder<UnknownNewsSourceNotification> builder)
        {
            builder.ToTable("unknown_news_source_notifications");

            builder.Ignore(unsnr => unsnr.DomainEvents);

            builder.HasKey(unsnr => unsnr.Id);
            builder.Property(unsnr => unsnr.Id)
                .ValueGeneratedOnAdd();

            builder.Property(unsnr => unsnr.Email)
                .IsRequired();

            builder.Property(unsnr => unsnr.RequestedAt)
                .IsRequired();

            builder
                .HasOne(r => r.UnknownNewsSource)
                .WithMany(r => r.NotificationQueue)
                .IsRequired();
        }
    }
}
