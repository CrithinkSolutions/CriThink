using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class UserSearchEntityConfiguration : IEntityTypeConfiguration<UserSearch>
    {
        public void Configure(EntityTypeBuilder<UserSearch> builder)
        {
            builder.Property(us => us.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));
        }
    }
}
