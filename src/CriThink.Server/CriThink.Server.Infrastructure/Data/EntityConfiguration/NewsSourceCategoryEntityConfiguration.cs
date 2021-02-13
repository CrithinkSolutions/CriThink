using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class NewsSourceCategoryEntityConfiguration : IEntityTypeConfiguration<NewsSourceCategory>
    {
        public void Configure(EntityTypeBuilder<NewsSourceCategory> builder)
        {
            builder.Property(nsc => nsc.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => EntityEnumConverter.GetEnumValue<NewsSourceAuthenticity>(stringValue));
        }
    }
}
