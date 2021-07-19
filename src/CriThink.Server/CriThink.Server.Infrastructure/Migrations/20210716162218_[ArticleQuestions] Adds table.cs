using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class ArticleQuestionsAddstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "article_questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    question = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article_questions", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "article_questions",
                columns: new[] { "id", "category", "question" },
                values: new object[,]
                {
                    { new Guid("30c7d606-d1cf-434e-a1f3-b7b1841dc331"), "General", "HQuestion" },
                    { new Guid("12f7218c-e2dd-43cd-82e0-a9216fcf6aff"), "General", "EQuestion" },
                    { new Guid("a05d4433-2c47-4749-a8f0-fb9a6e35a868"), "General", "AQuestion" },
                    { new Guid("8731f45c-a41f-45d9-b97d-0f181e2cce94"), "General", "DQuestion" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article_questions");
        }
    }
}
