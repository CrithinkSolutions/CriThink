using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriThink.Server.Infrastructure.Data.EntityConfiguration
{
    internal class NewsSourcePostAnswerAnswerEntityConfiguration : IEntityTypeConfiguration<NewsSourcePostAnswer>
    {
        public void Configure(EntityTypeBuilder<NewsSourcePostAnswer> builder)
        {
            builder.ToTable("news_source_post_answers");

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
