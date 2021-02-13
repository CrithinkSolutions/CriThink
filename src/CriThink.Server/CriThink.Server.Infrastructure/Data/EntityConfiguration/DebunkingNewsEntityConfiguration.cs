using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class DebunkingNewsEntityConfiguration : IEntityTypeConfiguration<DebunkingNews>
    {
        public void Configure(EntityTypeBuilder<DebunkingNews> builder)
        {
            builder.HasIndex(dn => dn.Link).IsUnique();
        }
    }
}
