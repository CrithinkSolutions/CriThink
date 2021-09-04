using CriThink.Server.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class ArticleAnswerEntityConfiguration : IEntityTypeConfiguration<ArticleAnswer>
    {
        public void Configure(EntityTypeBuilder<ArticleAnswer> builder)
        {
            builder.Ignore(aa => aa.DomainEvents);

            builder.HasKey(aa => aa.Id);
            builder.Property(aa => aa.Id)
                .ValueGeneratedOnAdd();

            builder.Property(aa => aa.Rate)
                .IsRequired();

            builder.Property(aa => aa.NewsLink)
                .IsRequired();

            builder
                .HasOne(aa => aa.User)
                .WithMany(u => u.ArticleAnswers)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
