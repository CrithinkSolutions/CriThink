using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class DebunkingNewsPublisherAddsseeddataforFullFact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "debunking_news_publishers",
                columns: new[] { "id", "country_id", "description", "facebook_page", "instagram_profile", "language_id", "link", "name", "opinion", "twitter_profile" },
                values: new object[] { new Guid("511199ed-595c-4830-a40b-bcb58ca7bbb2"), new Guid("812361b1-d1c3-4315-b601-4e060364a1d6"), "", "https://www.facebook.com/FullFact.org/", "https://www.instagram.com/fullfactorg/", new Guid("cea0eeea-ec03-483e-be0f-e2f1af7669d8"), "https://fullfact.org/", "FullFact", "", "https://twitter.com/fullfact" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "debunking_news_publishers",
                keyColumn: "id",
                keyValue: new Guid("511199ed-595c-4830-a40b-bcb58ca7bbb2"));
        }
    }
}
