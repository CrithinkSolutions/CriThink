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

        public DbSet<DebunkingNewsCountry> DebunkingNewsCountries { get; set; }

        public DbSet<DebunkingNewsLanguage> DebunkingNewsLanguages { get; set; }

        public DbSet<DebunkingNewsPublisher> DebunkingNewsPublishers { get; set; }


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

            #region DebunkingNews

            var countryItaly = new DebunkingNewsCountry
            {
                Id = Guid.Parse("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                Name = "Italy",
                Code = "it",
            };
            var countryUsa = new DebunkingNewsCountry
            {
                Id = Guid.Parse("3bd76fc5-5463-4194-b4f9-df111f7c294f"),
                Name = "United States of America",
                Code = "us",
            };
            var countryUk = new DebunkingNewsCountry
            {
                Id = Guid.Parse("812361b1-d1c3-4315-b601-4e060364a1d6"),
                Name = "United Kingdom",
                Code = "uk",
            };
            builder.Entity<DebunkingNewsCountry>(typeBuilder =>
            {
                typeBuilder.HasIndex(c => c.Code).IsUnique();
                typeBuilder.HasData((IEnumerable<DebunkingNewsCountry>) new[] { countryItaly, countryUsa, countryUk });
            });

            var languageItalian = new DebunkingNewsLanguage
            {
                Id = Guid.Parse("b5165f46-b82e-46c3-9b98-e5a37a10276f"),
                Code = EntityConstants.LanguageCodeIt,
                Name = "Italian",
            };
            var languageEnglish = new DebunkingNewsLanguage
            {
                Id = Guid.Parse("cea0eeea-ec03-483e-be0f-e2f1af7669d8"),
                Code = EntityConstants.LanguageCodeEn,
                Name = "English",
            };
            builder.Entity<DebunkingNewsLanguage>(typeBuilder =>
            {
                typeBuilder.HasIndex(l => l.Code).IsUnique();
                typeBuilder.HasData(languageItalian, languageEnglish);
            });

            builder.Entity<DebunkingNewsPublisher>(typeBuilder =>
            {
                typeBuilder
                    .HasMany(p => p.DebunkingNews)
                    .WithOne(dn => dn.Publisher)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

                typeBuilder
                    .HasOne(p => p.Language)
                    .WithMany(l => l.Publishers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

                typeBuilder
                    .HasOne(p => p.Country)
                    .WithMany(c => c.Publishers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

                typeBuilder.HasIndex(p => p.Name).IsUnique();

                typeBuilder.HasData(new[]
                {
                    new
                    {
                        Id = Guid.Parse("ec22b726-c503-4bfc-ae33-eb6729b22bef"),
                        Name = EntityConstants.OpenOnline,
                        Link = EntityConstants.OpenOnlineLink,
                        Description = EntityConstants.OpenOnlineDescription,
                        Opinion = EntityConstants.OpenOnlineOpinion,
                        FacebookPage = EntityConstants.OpenOnlineFacebook,
                        InstagramProfile = EntityConstants.OpenOnlineInstagram,
                        TwitterProfile = EntityConstants.OpenOnlineTwitter,
                        CountryId = countryItaly.Id,
                        LanguageId = languageItalian.Id,
                    }
                });
            });

            builder.Entity<DebunkingNews>(typeBuilder =>
            {
                typeBuilder.HasIndex(dn => dn.Link).IsUnique();
            });

            #endregion
        }

        private static TEnum GetEnumValue<TEnum>(string value)
            where TEnum : Enum
        {
            return (TEnum) Enum.Parse(typeof(TEnum), value);
        }
    }
}
