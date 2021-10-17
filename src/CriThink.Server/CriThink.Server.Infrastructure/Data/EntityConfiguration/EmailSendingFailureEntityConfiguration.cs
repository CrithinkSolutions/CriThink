using System.Collections.Generic;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class EmailSendingFailureEntityConfiguration : IEntityTypeConfiguration<EmailSendingFailure>
    {
        public void Configure(EntityTypeBuilder<EmailSendingFailure> builder)
        {
            builder.ToTable("email_sending_failures");

            builder.Ignore(dn => dn.DomainEvents);

            builder.HasKey(dn => dn.Id);
            builder.Property(dn => dn.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dn => dn.FromAddress);

            builder.Property(dn => dn.Recipients)
                .HasConversion(
                    value => EntityJsonConverter.ToJson(value),
                    jsonValue => EntityJsonConverter.GetFromJson<ICollection<string>>(jsonValue));

            builder.Property(dn => dn.HtmlBody);

            builder.Property(dn => dn.Subject);

            builder.Property(dn => dn.ExceptionMessage);
        }
    }
}
