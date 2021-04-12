using System;
using System.Linq;
using CriThink.Server.Core.Entities;
using CriThink.Server.Infrastructure.Data.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Infrastructure.Data
{
    public class CriThinkDbContext : IdentityDbContext<User, UserRole, Guid>
    {
        private readonly IOptions<User> _userOptions;
        private readonly IOptions<UserRole> _roleOptions;

        public CriThinkDbContext(DbContextOptions<CriThinkDbContext> context, IOptions<User> userOptions, IOptions<UserRole> roleOptions)
            : base(context)
        {
            _userOptions = userOptions /*?? throw new ArgumentNullException(nameof(userOptions))*/;
            _roleOptions = roleOptions /*?? throw new ArgumentNullException(nameof(roleOptions))*/;
        }

        public DbSet<NewsSourceCategory> NewsSourceCategories { get; set; }

        public DbSet<DebunkingNews> DebunkingNews { get; set; }

        public DbSet<DebunkingNewsTriggerLog> DebunkingNewsTriggerLogs { get; set; }

        public DbSet<DebunkingNewsCountry> DebunkingNewsCountries { get; set; }

        public DbSet<DebunkingNewsLanguage> DebunkingNewsLanguages { get; set; }

        public DbSet<DebunkingNewsPublisher> DebunkingNewsPublishers { get; set; }

        public DbSet<UnknownNewsSource> UnknownNewsSources { get; set; }

        public DbSet<UnknownNewsSourceNotificationRequest> UnknownNewsSourceNotificationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new DebunkingNewsCountryEntityConfiguration());
            builder.ApplyConfiguration(new DebunkingNewsEntityConfiguration());
            builder.ApplyConfiguration(new DebunkingNewsLanguageEntityConfiguration());
            builder.ApplyConfiguration(new DebunkingNewsPublisherEntityConfiguration());
            builder.ApplyConfiguration(new IdentityRoleClaimEntityConfiguration());
            builder.ApplyConfiguration(new IdentityUserClaimEntityConfiguration(_userOptions, _roleOptions));
            builder.ApplyConfiguration(new IdentityUserLoginEntityConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleEntityConfiguration(_userOptions, _roleOptions));
            builder.ApplyConfiguration(new IdentityUserTokenEntityConfiguration());
            builder.ApplyConfiguration(new NewsSourceCategoryEntityConfiguration());
            builder.ApplyConfiguration(new UnknownNewsSourceEntityConfiguration());
            builder.ApplyConfiguration(new UserEntityConfiguration(_userOptions));
            builder.ApplyConfiguration(new UserRoleEntityConfiguration(_roleOptions));
        }
    }

    public class CriThinkDbContextFactory : IDesignTimeDbContextFactory<CriThinkDbContext>
    {
        public CriThinkDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CriThinkDbContext>();
            if (args?.Any() == true)
            {
                var cs = args[0];
                Console.WriteLine($"------------------- {cs} -------------------");
            }

            var env = Environment.GetEnvironmentVariable("STAGING_CRITHINK_SERVER_CONNECTIONSTRINGS_CRITHINKDBPGSQLCONNECTION");
            if (!string.IsNullOrWhiteSpace(env))
            {
                Console.WriteLine($"------------------- {env} -------------------");
            }

            optionsBuilder.UseNpgsql(env).UseSnakeCaseNamingConvention(System.Globalization.CultureInfo.InvariantCulture); ;

            return new CriThinkDbContext(optionsBuilder.Options, null, null);
        }
    }
}
