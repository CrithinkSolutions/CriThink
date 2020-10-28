using System;
using System.Diagnostics.CodeAnalysis;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CriThink.Server.Infrastructure.Data
{
    public class CriThinkDbContext : IdentityDbContext<User, UserRole, Guid>
    {
        public CriThinkDbContext(DbContextOptions<CriThinkDbContext> context)
            : base(context)
        { }

        public DbSet<DemoNews> DemoNews { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        public DbSet<NewsSourceCategory> NewsSourceCategories { get; set; }

        public DbSet<DebunkingNews> DebunkingNews { get; set; }

        public DbSet<DebunkingNewsTriggerLog> DebunkingNewsTriggerLogs { get; set; }

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Injected")]
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(typeBuilder =>
            {
                typeBuilder.ToTable("Users");
                typeBuilder.Ignore(property => property.TwoFactorEnabled);
                typeBuilder.Ignore(property => property.PhoneNumberConfirmed);
            });

            builder.Entity<UserRole>(typeBuilder =>
            {
                typeBuilder.ToTable("UserRoles");
            });

            builder.Entity<NewsSourceCategory>()
                .Property(nsc => nsc.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => GetEnumValue<NewsSourceAuthenticity>(stringValue)
                );
        }

        private static TEnum GetEnumValue<TEnum>(string value)
            where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
    }
}
