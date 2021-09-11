using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsTriggerLogEntityConfiguration : IEntityTypeConfiguration<DebunkingNewsTriggerLog>
    {
        public void Configure(EntityTypeBuilder<DebunkingNewsTriggerLog> builder)
        {
            builder.ToTable("debunking_news_trigger_logs");

            builder.Ignore(dn => dn.DomainEvents);

            builder.HasKey(dntl => dntl.Id);
            builder.Property(dntl => dntl.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dntl => dntl.Status)
                .IsRequired()
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<DebunkingNewsTriggerLogStatus>(stringValue));

            builder.Property(dntl => dntl.TimeStamp)
                .IsRequired();

            builder.Property(dntl => dntl.Failures);
        }
    }
}
