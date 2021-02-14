using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class DebunkingNewsPublisherAddsChannel4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "debunking_news_publishers",
                columns: new[] { "id", "country_id", "description", "facebook_page", "instagram_profile", "language_id", "link", "name", "opinion", "twitter_profile" },
                values: new object[] { new Guid("3181faf4-45e2-4a91-8340-8ed9598513c8"), new Guid("812361b1-d1c3-4315-b601-4e060364a1d6"), "", "https://www.facebook.com/Channel4News", "https://www.instagram.com/channel4news/", new Guid("cea0eeea-ec03-483e-be0f-e2f1af7669d8"), "https://www.channel4.com/", "Channel4", "", "https://twitter.com/Channel4News" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "debunking_news_publishers",
                keyColumn: "id",
                keyValue: new Guid("3181faf4-45e2-4a91-8340-8ed9598513c8"));
        }
    }
}
