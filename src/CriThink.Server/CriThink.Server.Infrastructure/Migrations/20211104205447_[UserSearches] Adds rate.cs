using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class UserSearchesAddsrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "news_source_post_answers");

            migrationBuilder.AddColumn<decimal>(
                name: "rate",
                table: "user_searches",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rate",
                table: "user_searches");

            migrationBuilder.CreateTable(
                name: "news_source_post_answers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    news_link = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "ix_news_source_post_answers_user_id",
                table: "news_source_post_answers",
                column: "user_id");
        }
    }
}
