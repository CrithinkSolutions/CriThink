using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.Ignore(dn => dn.DomainEvents);
            builder.Ignore(dn => dn.Active);

            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Id)
                .ValueGeneratedOnAdd();

            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            builder.Property(rt => rt.Token)
                .IsRequired();

            builder.Property(rt => rt.RemoteIpAddress);

            builder
                .HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
