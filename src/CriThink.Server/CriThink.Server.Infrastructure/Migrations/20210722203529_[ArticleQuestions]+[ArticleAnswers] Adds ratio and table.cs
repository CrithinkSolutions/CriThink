using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class ArticleQuestionsArticleAnswersAddsratioandtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ratio",
                table: "article_questions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "article_answers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    news_link = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_article_answers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "article_questions",
                keyColumn: "id",
                keyValue: new Guid("12f7218c-e2dd-43cd-82e0-a9216fcf6aff"),
                column: "ratio",
                value: 0.4m);

            migrationBuilder.UpdateData(
                table: "article_questions",
                keyColumn: "id",
                keyValue: new Guid("30c7d606-d1cf-434e-a1f3-b7b1841dc331"),
                column: "ratio",
                value: 0.3m);

            migrationBuilder.UpdateData(
                table: "article_questions",
                keyColumn: "id",
                keyValue: new Guid("8731f45c-a41f-45d9-b97d-0f181e2cce94"),
                column: "ratio",
                value: 0.1m);

            migrationBuilder.UpdateData(
                table: "article_questions",
                keyColumn: "id",
                keyValue: new Guid("a05d4433-2c47-4749-a8f0-fb9a6e35a868"),
                column: "ratio",
                value: 0.2m);

            migrationBuilder.CreateIndex(
                name: "ix_article_answers_user_id",
                table: "article_answers",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article_answers");

            migrationBuilder.DropColumn(
                name: "ratio",
                table: "article_questions");
        }
    }
}
