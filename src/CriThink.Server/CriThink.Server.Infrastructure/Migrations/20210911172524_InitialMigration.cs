using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "debunking_news_countries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_debunking_news_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "debunking_news_languages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_debunking_news_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "debunking_news_trigger_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    time_stamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    failures = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_debunking_news_trigger_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "news_source_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    authenticity = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news_source_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "news_source_post_questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    question = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    ratio = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news_source_post_questions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unknown_news_sources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false),
                    first_requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    identified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    request_count = table.Column<int>(type: "integer", nullable: false),
                    authenticity = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unknown_news_sources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    deletion_requested_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deletion_scheduled_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "debunking_news_publishers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    link = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    opinion = table.Column<string>(type: "text", nullable: true),
                    facebook_page = table.Column<string>(type: "text", nullable: true),
                    instagram_profile = table.Column<string>(type: "text", nullable: true),
                    twitter_profile = table.Column<string>(type: "text", nullable: true),
                    country_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_debunking_news_publishers", x => x.id);
                    table.ForeignKey(
                        name: "fk_debunking_news_publishers_debunking_news_countries_country_",
                        column: x => x.country_id,
                        principalTable: "debunking_news_countries",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_debunking_news_publishers_debunking_news_languages_language",
                        column: x => x.language_id,
                        principalTable: "debunking_news_languages",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "unknown_news_source_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    unknown_news_source_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unknown_news_source_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_unknown_news_source_notifications_unknown_news_sources_unkn",
                        column: x => x.unknown_news_source_id,
                        principalTable: "unknown_news_sources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnet_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aspnet_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_aspnet_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnet_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aspnet_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_aspnet_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnet_user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aspnet_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_aspnet_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnet_user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aspnet_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_aspnet_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_aspnet_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnet_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aspnet_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_aspnet_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_source_post_answers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    news_link = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news_source_post_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_news_source_post_answers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    remote_ip_address = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    given_name = table.Column<string>(type: "text", nullable: true),
                    family_name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<string>(type: "text", nullable: true),
                    avatar_path = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    telegram = table.Column<string>(type: "text", nullable: true),
                    skype = table.Column<string>(type: "text", nullable: true),
                    twitter = table.Column<string>(type: "text", nullable: true),
                    instagram = table.Column<string>(type: "text", nullable: true),
                    facebook = table.Column<string>(type: "text", nullable: true),
                    snapchat = table.Column<string>(type: "text", nullable: true),
                    youtube = table.Column<string>(type: "text", nullable: true),
                    blog = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "Date", nullable: true),
                    registered_on = table.Column<DateTime>(type: "Date", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_searches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    news_link = table.Column<string>(type: "text", nullable: false),
                    authenticity = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_searches", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_searches_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "debunking_news",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    publishing_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    link = table.Column<string>(type: "text", nullable: false),
                    image_link = table.Column<string>(type: "text", nullable: true),
                    news_caption = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    keywords = table.Column<string>(type: "text", nullable: true),
                    publisher_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_debunking_news", x => x.id);
                    table.ForeignKey(
                        name: "fk_debunking_news_debunking_news_publishers_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "debunking_news_publishers",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "debunking_news_countries",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("575003e3-991c-4ac5-9ce4-3399553f64a7"), "it", "Italy" },
                    { new Guid("3bd76fc5-5463-4194-b4f9-df111f7c294f"), "us", "United States of America" },
                    { new Guid("812361b1-d1c3-4315-b601-4e060364a1d6"), "uk", "United Kingdom" }
                });

            migrationBuilder.InsertData(
                table: "debunking_news_languages",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("b5165f46-b82e-46c3-9b98-e5a37a10276f"), "it", "Italian" },
                    { new Guid("cea0eeea-ec03-483e-be0f-e2f1af7669d8"), "en", "English" }
                });

            migrationBuilder.InsertData(
                table: "news_source_categories",
                columns: new[] { "id", "authenticity", "description" },
                values: new object[,]
                {
                    { new Guid("d66bf55d-d30d-448f-be69-d2e0cebdd26a"), "SocialMedia", "NewsSourceSocialMedia" },
                    { new Guid("3cc36977-5115-45ea-88e9-7c86f19b6cd6"), "Suspicious", "NewsSourceSuspicious" },
                    { new Guid("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"), "Conspiracist", "NewsSourceConspiracist" },
                    { new Guid("762f6747-e0de-4b3b-80b1-46d599ce02df"), "Satirical", "NewsSourceSatirical" },
                    { new Guid("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"), "FakeNews", "NewsSourceFakeNews" },
                    { new Guid("6254c650-76f0-4bba-ba5e-01218891f729"), "Reliable", "NewsSourceReliable" }
                });

            migrationBuilder.InsertData(
                table: "news_source_post_questions",
                columns: new[] { "id", "category", "question", "ratio" },
                values: new object[,]
                {
                    { new Guid("30c7d606-d1cf-434e-a1f3-b7b1841dc331"), "General", "HQuestion", 0.3m },
                    { new Guid("12f7218c-e2dd-43cd-82e0-a9216fcf6aff"), "General", "EQuestion", 0.4m },
                    { new Guid("a05d4433-2c47-4749-a8f0-fb9a6e35a868"), "General", "AQuestion", 0.2m },
                    { new Guid("8731f45c-a41f-45d9-b97d-0f181e2cce94"), "General", "DQuestion", 0.1m }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"), "15b1b12c-4dff-413e-81d5-7c9423f25c35", "Admin", "ADMIN" },
                    { new Guid("4c28eed7-a34a-4534-9c2c-5ffe86b72393"), "4E597EE6-5339-44B0-988E-258AD486BE49", "FreeUser", "FREEUSER" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "access_failed_count", "concurrency_stamp", "email", "email_confirmed", "lockout_enabled", "lockout_end", "normalized_email", "normalized_user_name", "password_hash", "phone_number", "security_stamp", "user_name", "deletion_requested_on", "deletion_scheduled_on" },
                values: new object[] { new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"), 0, "c31844c9-d81b-4c66-991c-a60b0ba36f76", "service@crithink.com", true, false, null, "SERVICE@CRITHINK.COM", "SERVICE", "AQAAAAEAACcQAAAAEDw0jwJ7LHQhBe2Zo45PpE6FYSpNsPyHbXP/YD51WzHrmI0MAbwHhdZf6MytihsYzg==", null, "XV7NZ5BSN7ASJO6OMO3WT2L75Y2TI6VD", "service", null, null });

            migrationBuilder.InsertData(
                table: "aspnet_user_claims",
                columns: new[] { "id", "claim_type", "claim_value", "user_id" },
                values: new object[,]
                {
                    { 1, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "f62fc754-e296-4aca-0a3f-08d88b1daff7", new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7") },
                    { 2, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "service@crithink.com", new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7") },
                    { 3, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "service", new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7") },
                    { 4, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7") }
                });

            migrationBuilder.InsertData(
                table: "aspnet_user_roles",
                columns: new[] { "role_id", "user_id" },
                values: new object[] { new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"), new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7") });

            migrationBuilder.InsertData(
                table: "debunking_news_publishers",
                columns: new[] { "id", "country_id", "description", "facebook_page", "instagram_profile", "language_id", "link", "name", "opinion", "twitter_profile" },
                values: new object[,]
                {
                    { new Guid("ec22b726-c503-4bfc-ae33-eb6729b22bef"), new Guid("575003e3-991c-4ac5-9ce4-3399553f64a7"), "", "https://www.facebook.com/Opengiornaleonline/", "https://www.instagram.com/open_giornaleonline/", new Guid("b5165f46-b82e-46c3-9b98-e5a37a10276f"), "https://www.open.online/", "Open", "", "https://twitter.com/open_gol" },
                    { new Guid("80aa1eaf-d64b-46eb-a438-503b716f9c2a"), new Guid("575003e3-991c-4ac5-9ce4-3399553f64a7"), "", "https://www.facebook.com/facta.news", "https://www.instagram.com/facta.news/", new Guid("b5165f46-b82e-46c3-9b98-e5a37a10276f"), "https://facta.news/", "FactaNews", "", "https://twitter.com/FactaNews" },
                    { new Guid("3181faf4-45e2-4a91-8340-8ed9598513c8"), new Guid("812361b1-d1c3-4315-b601-4e060364a1d6"), "", "https://www.facebook.com/Channel4News", "https://www.instagram.com/channel4news/", new Guid("cea0eeea-ec03-483e-be0f-e2f1af7669d8"), "https://www.channel4.com/", "Channel4", "", "https://twitter.com/Channel4News" },
                    { new Guid("511199ed-595c-4830-a40b-bcb58ca7bbb2"), new Guid("812361b1-d1c3-4315-b601-4e060364a1d6"), "", "https://www.facebook.com/FullFact.org/", "https://www.instagram.com/fullfactorg/", new Guid("cea0eeea-ec03-483e-be0f-e2f1af7669d8"), "https://fullfact.org/", "FullFact", "", "https://twitter.com/fullfact" }
                });

            migrationBuilder.InsertData(
                table: "user_profiles",
                columns: new[] { "id", "avatar_path", "blog", "country", "date_of_birth", "description", "facebook", "family_name", "gender", "given_name", "instagram", "registered_on", "skype", "snapchat", "telegram", "twitter", "user_id", "youtube" },
                values: new object[] { new Guid("cb825a64-9cdb-48e7-8bb0-45d5bed6eee2"), null, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is the default account", null, null, null, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"), null });

            migrationBuilder.CreateIndex(
                name: "ix_aspnet_role_claims_role_id",
                table: "aspnet_role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_aspnet_user_claims_user_id",
                table: "aspnet_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_aspnet_user_logins_user_id",
                table: "aspnet_user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_aspnet_user_roles_role_id",
                table: "aspnet_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_link",
                table: "debunking_news",
                column: "link",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_publisher_id",
                table: "debunking_news",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_countries_code",
                table: "debunking_news_countries",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_languages_code",
                table: "debunking_news_languages",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_publishers_country_id",
                table: "debunking_news_publishers",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_publishers_language_id",
                table: "debunking_news_publishers",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_publishers_name",
                table: "debunking_news_publishers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_news_source_categories_authenticity",
                table: "news_source_categories",
                column: "authenticity",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_news_source_post_answers_user_id",
                table: "news_source_post_answers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_unknown_news_source_notifications_unknown_news_source_id",
                table: "unknown_news_source_notifications",
                column: "unknown_news_source_id");

            migrationBuilder.CreateIndex(
                name: "ix_unknown_news_sources_uri",
                table: "unknown_news_sources",
                column: "uri",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_user_id",
                table: "user_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "user_roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_searches_user_id",
                table: "user_searches",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "users",
                column: "normalized_user_name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aspnet_role_claims");

            migrationBuilder.DropTable(
                name: "aspnet_user_claims");

            migrationBuilder.DropTable(
                name: "aspnet_user_logins");

            migrationBuilder.DropTable(
                name: "aspnet_user_roles");

            migrationBuilder.DropTable(
                name: "aspnet_user_tokens");

            migrationBuilder.DropTable(
                name: "debunking_news");

            migrationBuilder.DropTable(
                name: "debunking_news_trigger_logs");

            migrationBuilder.DropTable(
                name: "news_source_categories");

            migrationBuilder.DropTable(
                name: "news_source_post_answers");

            migrationBuilder.DropTable(
                name: "news_source_post_questions");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "unknown_news_source_notifications");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "user_searches");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "debunking_news_publishers");

            migrationBuilder.DropTable(
                name: "unknown_news_sources");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "debunking_news_countries");

            migrationBuilder.DropTable(
                name: "debunking_news_languages");
        }
    }
}
