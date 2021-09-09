using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Core.Entities;
using CriThink.Server.Core.Repositories;
using CriThink.Server.Infrastructure.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Data
{
    public class CriThinkDbContext : IdentityDbContext<User, UserRole, Guid>, ICriThinkDbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public CriThinkDbContext(
            DbContextOptions<CriThinkDbContext> options,
            IMediator mediator)
            : base(options)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
        }

        internal CriThinkDbContext(
            DbContextOptions<CriThinkDbContext> options)
            : base(options)
        { }

        public DbSet<NewsSourcePostAnswer> NewsSourcePostAnswers { get; set; }

        public DbSet<NewsSourcePostQuestion> NewsSourcePostQuestions { get; set; }

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

        public DbSet<UnknownNewsSourceNotification> UnknownNewsSourceNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.Load("CriThink.Server.Infrastructure"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseNpgsql();
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await _mediator?.DispatchDomainEventsAsync(this);

            return result >= 0;
        }
    }
}
