using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class DebunkingNewsPublisherAddseeddataforFactaNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "debunking_news_publishers",
                columns: new[] { "id", "country_id", "description", "facebook_page", "instagram_profile", "language_id", "link", "name", "opinion", "twitter_profile" },
                values: new object[] { new Guid("80aa1eaf-d64b-46eb-a438-503b716f9c2a"), new Guid("575003e3-991c-4ac5-9ce4-3399553f64a7"), "", "https://www.facebook.com/facta.news", "https://www.instagram.com/facta.news/", new Guid("b5165f46-b82e-46c3-9b98-e5a37a10276f"), "https://facta.news/", "FactaNews", "", "https://twitter.com/FactaNews" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "debunking_news_publishers",
                keyColumn: "id",
                keyValue: new Guid("80aa1eaf-d64b-46eb-a438-503b716f9c2a"));
        }
    }
}
