﻿// <auto-generated />
using System;
using CriThink.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CriThink.Server.Infrastructure.Migrations
{
    [DbContext(typeof(CriThinkDbContext))]
    [Migration("20210329101909_[Demo]+[Questions] Removes tables")]
    partial class DemoQuestionsRemovestables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNews", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ImageLink")
                        .HasColumnType("text")
                        .HasColumnName("image_link");

                    b.Property<string>("Keywords")
                        .HasColumnType("text")
                        .HasColumnName("keywords");

                    b.Property<string>("Link")
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<string>("NewsCaption")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("news_caption");

                    b.Property<Guid>("PublisherId")
                        .HasColumnType("uuid")
                        .HasColumnName("publisher_id");

                    b.Property<DateTime>("PublishingDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("publishing_date");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_debunking_news");

                    b.HasIndex("Link")
                        .IsUnique()
                        .HasDatabaseName("ix_debunking_news_link");

                    b.HasIndex("PublisherId")
                        .HasDatabaseName("ix_debunking_news_publisher_id");

                    b.ToTable("debunking_news");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsCountry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_debunking_news_countries");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_debunking_news_countries_code");

                    b.ToTable("debunking_news_countries");

                    b.HasData(
                        new
                        {
                            Id = new Guid("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                            Code = "it",
                            Name = "Italy"
                        },
                        new
                        {
                            Id = new Guid("3bd76fc5-5463-4194-b4f9-df111f7c294f"),
                            Code = "us",
                            Name = "United States of America"
                        },
                        new
                        {
                            Id = new Guid("812361b1-d1c3-4315-b601-4e060364a1d6"),
                            Code = "uk",
                            Name = "United Kingdom"
                        });
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsLanguage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_debunking_news_languages");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_debunking_news_languages_code");

                    b.ToTable("debunking_news_languages");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b5165f46-b82e-46c3-9b98-e5a37a10276f"),
                            Code = "it",
                            Name = "Italian"
                        },
                        new
                        {
                            Id = new Guid("cea0eeea-ec03-483e-be0f-e2f1af7669d8"),
                            Code = "en",
                            Name = "English"
                        });
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsPublisher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid")
                        .HasColumnName("country_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("FacebookPage")
                        .HasColumnType("text")
                        .HasColumnName("facebook_page");

                    b.Property<string>("InstagramProfile")
                        .HasColumnType("text")
                        .HasColumnName("instagram_profile");

                    b.Property<Guid>("LanguageId")
                        .HasColumnType("uuid")
                        .HasColumnName("language_id");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Opinion")
                        .HasColumnType("text")
                        .HasColumnName("opinion");

                    b.Property<string>("TwitterProfile")
                        .HasColumnType("text")
                        .HasColumnName("twitter_profile");

                    b.HasKey("Id")
                        .HasName("pk_debunking_news_publishers");

                    b.HasIndex("CountryId")
                        .HasDatabaseName("ix_debunking_news_publishers_country_id");

                    b.HasIndex("LanguageId")
                        .HasDatabaseName("ix_debunking_news_publishers_language_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_debunking_news_publishers_name");

                    b.ToTable("debunking_news_publishers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ec22b726-c503-4bfc-ae33-eb6729b22bef"),
                            CountryId = new Guid("575003e3-991c-4ac5-9ce4-3399553f64a7"),
                            Description = "",
                            FacebookPage = "https://www.facebook.com/Opengiornaleonline/",
                            InstagramProfile = "https://www.instagram.com/open_giornaleonline/",
                            LanguageId = new Guid("b5165f46-b82e-46c3-9b98-e5a37a10276f"),
                            Link = "https://www.open.online/",
                            Name = "Open",
                            Opinion = "",
                            TwitterProfile = "https://twitter.com/open_gol"
                        },
                        new
                        {
                            Id = new Guid("3181faf4-45e2-4a91-8340-8ed9598513c8"),
                            CountryId = new Guid("812361b1-d1c3-4315-b601-4e060364a1d6"),
                            Description = "",
                            FacebookPage = "https://www.facebook.com/Channel4News",
                            InstagramProfile = "https://www.instagram.com/channel4news/",
                            LanguageId = new Guid("cea0eeea-ec03-483e-be0f-e2f1af7669d8"),
                            Link = "https://www.channel4.com/",
                            Name = "Channel4",
                            Opinion = "",
                            TwitterProfile = "https://twitter.com/Channel4News"
                        });
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsTriggerLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("FailReason")
                        .HasColumnType("text")
                        .HasColumnName("fail_reason");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("boolean")
                        .HasColumnName("is_successful");

                    b.Property<string>("TimeStamp")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("time_stamp");

                    b.HasKey("Id")
                        .HasName("pk_debunking_news_trigger_logs");

                    b.ToTable("debunking_news_trigger_logs");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.NewsSourceCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Authenticity")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("authenticity");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_news_source_categories");

                    b.HasIndex("Authenticity")
                        .IsUnique()
                        .HasDatabaseName("ix_news_source_categories_authenticity");

                    b.ToTable("news_source_categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"),
                            Authenticity = "FakeNews",
                            Description = "Sources in this category show extreme bias, poor or no sourcing to credible information, a complete lack of transparency and/or publish fake news for profit or ideologically influence the audience. These sources may be very untrustworthy and should be fact checked."
                        },
                        new
                        {
                            Id = new Guid("762f6747-e0de-4b3b-80b1-46d599ce02df"),
                            Authenticity = "Satirical",
                            Description = "These sources exclusively use humor, irony, exaggeration, or ridicule to expose and criticize people’s stupidity or vices, particularly in the context of contemporary politics and other topical issues. Primarily these sources are clear that they are satire and do not attempt to deceive."
                        },
                        new
                        {
                            Id = new Guid("6254c650-76f0-4bba-ba5e-01218891f729"),
                            Authenticity = "Reliable",
                            Description = "These sources have minimal bias and use very few loaded words (wording that attempts to influence an audience by using appeal to emotion or stereotypes).  The reporting is factual and usually sourced."
                        },
                        new
                        {
                            Id = new Guid("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"),
                            Authenticity = "Conspiracist",
                            Description = "Sources in the Conspiracy category may publish unverifiable information that is not always supported by evidence. Actually, they usually publish Conspiracy theories consisting in explanations for an event or situation that invoke a conspiracy by sinister and powerful groups, often political in motivation. These sources may be untrustworthy for credible/verifiable information, therefore fact checking and further investigation is recommended."
                        },
                        new
                        {
                            Id = new Guid("3cc36977-5115-45ea-88e9-7c86f19b6cd6"),
                            Authenticity = "Suspicious",
                            Description = "These sources may publish a mix of non-factual and factual information which is misrepresented and/or reported with bias (e.g. political leaning bias). Therefore, information coming from these sources should be double-checked considering reliable sources of news."
                        },
                        new
                        {
                            Id = new Guid("d66bf55d-d30d-448f-be69-d2e0cebdd26a"),
                            Authenticity = "SocialMedia",
                            Description = "Social media platforms (such as Facebook, Twitter, Reddit and so on) cannot be classified strictly as sources of information. Rather these platforms are just mediums through which information can be shared. In these cases, verifying the reliability of the user/page sharing the information is recommended."
                        });
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.UnknownNewsSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Authenticity")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("authenticity");

                    b.Property<DateTime>("FirstRequestedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("first_requested_at");

                    b.Property<DateTime?>("IdentifiedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("identified_at");

                    b.Property<int>("RequestCount")
                        .HasColumnType("integer")
                        .HasColumnName("request_count");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("uri");

                    b.HasKey("Id")
                        .HasName("pk_unknown_news_sources");

                    b.HasIndex("Uri")
                        .IsUnique()
                        .HasDatabaseName("ix_unknown_news_sources_uri");

                    b.ToTable("unknown_news_sources");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.UnknownNewsSourceNotificationRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<DateTime>("RequestedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("requested_at");

                    b.Property<Guid>("UnknownNewsSourceId")
                        .HasColumnType("uuid")
                        .HasColumnName("unknown_news_source_id");

                    b.HasKey("Id")
                        .HasName("pk_unknown_news_source_notification_requests");

                    b.HasIndex("UnknownNewsSourceId")
                        .HasDatabaseName("ix_unknown_news_source_notification_requests_unknown_news_sour");

                    b.ToTable("unknown_news_source_notification_requests");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c31844c9-d81b-4c66-991c-a60b0ba36f76",
                            Email = "service@crithink.com",
                            EmailConfirmed = true,
                            IsDeleted = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "SERVICE@CRITHINK.COM",
                            NormalizedUserName = "SERVICE",
                            PasswordHash = "AQAAAAEAACcQAAAAEDw0jwJ7LHQhBe2Zo45PpE6FYSpNsPyHbXP/YD51WzHrmI0MAbwHhdZf6MytihsYzg==",
                            SecurityStamp = "XV7NZ5BSN7ASJO6OMO3WT2L75Y2TI6VD",
                            UserName = "service"
                        });
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("user_roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                            ConcurrencyStamp = "15b1b12c-4dff-413e-81d5-7c9423f25c35",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_role_claims_role_id");

                    b.ToTable("aspnet_role_claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_claims_user_id");

                    b.ToTable("aspnet_user_claims");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                            ClaimValue = "f62fc754-e296-4aca-0a3f-08d88b1daff7",
                            UserId = new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7")
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
                            ClaimValue = "service@crithink.com",
                            UserId = new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7")
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                            ClaimValue = "service",
                            UserId = new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7")
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                            ClaimValue = "Admin",
                            UserId = new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_logins_user_id");

                    b.ToTable("aspnet_user_logins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_user_roles_role_id");

                    b.ToTable("aspnet_user_roles");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                            RoleId = new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_user_tokens");

                    b.ToTable("aspnet_user_tokens");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNews", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.DebunkingNewsPublisher", "Publisher")
                        .WithMany("DebunkingNews")
                        .HasForeignKey("PublisherId")
                        .HasConstraintName("fk_debunking_news_debunking_news_publishers_publisher_id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsPublisher", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.DebunkingNewsCountry", "Country")
                        .WithMany("Publishers")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("fk_debunking_news_publishers_debunking_news_countries_country_")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CriThink.Server.Domain.Entities.DebunkingNewsLanguage", "Language")
                        .WithMany("Publishers")
                        .HasForeignKey("LanguageId")
                        .HasConstraintName("fk_debunking_news_publishers_debunking_news_languages_language")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.UnknownNewsSourceNotificationRequest", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.UnknownNewsSource", "UnknownNewsSource")
                        .WithMany("NotificationQueue")
                        .HasForeignKey("UnknownNewsSourceId")
                        .HasConstraintName("fk_unknown_news_source_notification_requests_unknown_news_sour")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UnknownNewsSource");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_role_claims_asp_net_roles_user_role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_claims_asp_net_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_logins_asp_net_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_user_roles_asp_net_roles_user_role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CriThink.Server.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_roles_asp_net_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("CriThink.Server.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_tokens_asp_net_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsCountry", b =>
                {
                    b.Navigation("Publishers");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsLanguage", b =>
                {
                    b.Navigation("Publishers");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.DebunkingNewsPublisher", b =>
                {
                    b.Navigation("DebunkingNews");
                });

            modelBuilder.Entity("CriThink.Server.Domain.Entities.UnknownNewsSource", b =>
                {
                    b.Navigation("NotificationQueue");
                });
#pragma warning restore 612, 618
        }
    }
}
