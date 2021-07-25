using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class ArticleAnswerEntityConfiguration : IEntityTypeConfiguration<ArticleAnswer>
    {
        public void Configure(EntityTypeBuilder<ArticleAnswer> builder)
        {
            builder
                .HasOne(aa => aa.User)
                .WithMany(u => u.ArticleAnswers)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
