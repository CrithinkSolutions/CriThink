using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using CriThink.Server.Core.Commands;
using CriThink.Server.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CriThink.Server.Infrastructure.Data
{
    public class CriThinkDbContext : IdentityDbContext<User, UserRole, Guid>
    {
        private readonly IOptions<User> _serviceUser;
        private readonly IOptions<UserRole> _adminRole;

        public CriThinkDbContext(DbContextOptions<CriThinkDbContext> context, IOptions<User> configuration, IOptions<UserRole> userRole)
            : base(context)
        {
            _serviceUser = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _adminRole = userRole ?? throw new ArgumentNullException(nameof(userRole));
        }

        public DbSet<DemoNews> DemoNews { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        public DbSet<NewsSourceCategory> NewsSourceCategories { get; set; }

        public DbSet<DebunkingNews> DebunkingNews { get; set; }

        public DbSet<DebunkingNewsTriggerLog> DebunkingNewsTriggerLogs { get; set; }

        public DbSet<UnknownSource> UnknownSources { get; set; }

        public DbSet<UnknownSourceNotificationRequest> UnknownSourceNotificationRequests { get; set; }

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Injected")]
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region User

            builder.Entity<User>(typeBuilder =>
            {
                typeBuilder.ToTable("users");
                typeBuilder.Ignore(property => property.TwoFactorEnabled);
                typeBuilder.Ignore(property => property.PhoneNumberConfirmed);
            });

            var serviceUser = _serviceUser.Value;
            builder.Entity<User>().HasData(serviceUser);

            #endregion

            #region UserRole

            builder.Entity<UserRole>(typeBuilder =>
            {
                typeBuilder.ToTable("user_roles");
            });

            var adminRole = _adminRole.Value;
            builder.Entity<UserRole>().HasData(adminRole);

            #endregion

            builder.Entity<IdentityUserRole<Guid>>(typeBuilder =>
            {
                typeBuilder.ToTable("aspnet_user_roles");
                typeBuilder.HasData(new IdentityUserRole<Guid>
                {
                    RoleId = adminRole.Id,
                    UserId = serviceUser.Id
                });
            });

            builder.Entity<IdentityUserClaim<Guid>>(typeBuilder =>
            {
                typeBuilder.ToTable("aspnet_user_claims");
                typeBuilder.HasData(new List<IdentityUserClaim<Guid>>
                {
                    new IdentityUserClaim<Guid>
                    {
                        UserId = serviceUser.Id,
                        ClaimType = ClaimTypes.NameIdentifier,
                        ClaimValue = serviceUser.Id.ToString(),
                        Id = 1
                    },
                    new IdentityUserClaim<Guid>
                    {
                        UserId = serviceUser.Id,
                        ClaimType = ClaimTypes.Email,
                        ClaimValue = serviceUser.Email,
                        Id = 2
                    },
                    new IdentityUserClaim<Guid>
                    {
                        UserId = serviceUser.Id,
                        ClaimType = ClaimTypes.Name,
                        ClaimValue = serviceUser.UserName,
                        Id = 3
                    },
                    new IdentityUserClaim<Guid>
                    {
                        UserId = serviceUser.Id,
                        ClaimType = ClaimTypes.Role,
                        ClaimValue = adminRole.Name,
                        Id = 4
                    }
                });
            });

            builder.Entity<IdentityRoleClaim<Guid>>(typeBuilder =>
            {
                typeBuilder.ToTable("aspnet_role_claims");
            });

            builder.Entity<IdentityUserLogin<Guid>>(typeBuilder =>
            {
                typeBuilder.ToTable("aspnet_user_logins");
            });

            builder.Entity<IdentityUserToken<Guid>>(typeBuilder =>
            {
                typeBuilder.ToTable("aspnet_user_tokens");
            });

            builder.Entity<NewsSourceCategory>()
                .Property(nsc => nsc.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => GetEnumValue<NewsSourceAuthenticity>(stringValue)
                );

            builder.Entity<DebunkingNews>()
                .HasIndex(dn => dn.Link)
                .IsUnique();

            builder.Entity<UnknownSource>()
                .Property(us => us.Authenticity)
                .HasConversion(
                    enumValue => enumValue.ToString(),
                    stringValue => GetEnumValue<NewsSourceAuthenticity>(stringValue)
                );

            builder.Entity<UnknownSource>()
                .HasIndex(us => us.Uri)
                .IsUnique();
        }

        private static TEnum GetEnumValue<TEnum>(string value)
            where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
    }
}
