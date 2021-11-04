using System.Threading;
using System.Threading.Tasks;
using CriThink.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Data
{
    public interface ICriThinkDbContext
    {
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
