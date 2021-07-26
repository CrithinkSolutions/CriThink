using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Data
{
    public interface ICriThinkDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class CriThinkDbContext : IdentityDbContext<User, UserRole, Guid>, IUnitOfWork
    {
        public CriThinkDbContext(DbContextOptions<CriThinkDbContext> options)
            : base(options)
        { }

        public DbSet<ArticleAnswer> ArticleAnswers { get; set; }

        public DbSet<ArticleQuestion> ArticleQuestions { get; set; }

        public DbSet<NewsSourceCategory> NewsSourceCategories { get; set; }

        public DbSet<DebunkingNews> DebunkingNews { get; set; }

        public DbSet<DebunkingNewsTriggerLog> DebunkingNewsTriggerLogs { get; set; }

        public DbSet<DebunkingNewsCountry> DebunkingNewsCountries { get; set; }

        public DbSet<DebunkingNewsLanguage> DebunkingNewsLanguages { get; set; }

        public DbSet<DebunkingNewsPublisher> DebunkingNewsPublishers { get; set; }

        public DbSet<UnknownNewsSource> UnknownNewsSources { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserSearch> UserSearches { get; set; }

        public DbSet<UnknownNewsSourceNotificationRequest> UnknownNewsSourceNotificationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.Load("CriThink.Server.Infrastructure"));
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // TODO:
            // await _mediator.DispatchDomainEventsAsync(this);

            var result = await base.SaveChangesAsync(cancellationToken);
            return result >= 0;
        }
    }
}
