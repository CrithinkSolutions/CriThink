using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class DebunkingNewsCountryLanguageProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "publisher_name",
                table: "debunking_news");

            migrationBuilder.AddColumn<Guid>(
                name: "publisher_id",
                table: "debunking_news",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                    language_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country_id = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                column: "concurrency_stamp",
                value: "15b1b12c-4dff-413e-81d5-7c9423f25c35");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                column: "concurrency_stamp",
                value: "c31844c9-d81b-4c66-991c-a60b0ba36f76");

            migrationBuilder.InsertData(
                table: "debunking_news_publishers",
                columns: new[] { "id", "country_id", "description", "facebook_page", "instagram_profile", "language_id", "link", "name", "opinion", "twitter_profile" },
                values: new object[] { new Guid("ec22b726-c503-4bfc-ae33-eb6729b22bef"), new Guid("575003e3-991c-4ac5-9ce4-3399553f64a7"), "", "https://www.facebook.com/Opengiornaleonline/", "https://www.instagram.com/open_giornaleonline/", new Guid("b5165f46-b82e-46c3-9b98-e5a37a10276f"), "https://www.open.online/", "Open", "", "https://twitter.com/open_gol" });

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

            migrationBuilder.AddForeignKey(
                name: "fk_debunking_news_debunking_news_publishers_publisher_id",
                table: "debunking_news",
                column: "publisher_id",
                principalTable: "debunking_news_publishers",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_debunking_news_debunking_news_publishers_publisher_id",
                table: "debunking_news");

            migrationBuilder.DropTable(
                name: "debunking_news_publishers");

            migrationBuilder.DropTable(
                name: "debunking_news_countries");

            migrationBuilder.DropTable(
                name: "debunking_news_languages");

            migrationBuilder.DropIndex(
                name: "ix_debunking_news_publisher_id",
                table: "debunking_news");

            migrationBuilder.DropColumn(
                name: "publisher_id",
                table: "debunking_news");

            migrationBuilder.AddColumn<string>(
                name: "publisher_name",
                table: "debunking_news",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                column: "concurrency_stamp",
                value: "b699027a-4237-4aeb-91bf-acc9ea577d46");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                column: "concurrency_stamp",
                value: "b4144e09-2e39-4788-83fc-77e89caaa123");
        }
    }
}
