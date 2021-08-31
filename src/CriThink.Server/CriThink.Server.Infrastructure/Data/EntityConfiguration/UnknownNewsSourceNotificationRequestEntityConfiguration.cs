using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UnknownNewsSourceNotificationRequestEntityConfiguration : IEntityTypeConfiguration<UnknownNewsSourceNotificationRequest>
    {
        public void Configure(EntityTypeBuilder<UnknownNewsSourceNotificationRequest> builder)
        {
            builder.Ignore(unsnr => unsnr.DomainEvents);

            builder.HasKey(unsnr => unsnr.Id);
            builder.Property(unsnr => unsnr.Id)
                .ValueGeneratedOnAdd();

            builder.Property(unsnr => unsnr.Email)
                .IsRequired();

            builder.Property(unsnr => unsnr.RequestedAt);

            builder.Property(unsnr => unsnr.UnknownNewsSource)
                .IsRequired();
        }
    }
}
