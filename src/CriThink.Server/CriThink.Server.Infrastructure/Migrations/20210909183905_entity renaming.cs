using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class entityrenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article_answers");

            migrationBuilder.DropTable(
                name: "unknown_news_source_notification_requests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_article_questions",
                table: "article_questions");

            migrationBuilder.RenameTable(
                name: "article_questions",
                newName: "news_source_post_questions");

            migrationBuilder.AddPrimaryKey(
                name: "pk_news_source_post_questions",
                table: "news_source_post_questions",
                column: "id");

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

            migrationBuilder.CreateIndex(
                name: "ix_news_source_post_answers_user_id",
                table: "news_source_post_answers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_unknown_news_source_notifications_unknown_news_source_id",
                table: "unknown_news_source_notifications",
                column: "unknown_news_source_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "news_source_post_answers");

            migrationBuilder.DropTable(
                name: "unknown_news_source_notifications");

            migrationBuilder.DropPrimaryKey(
                name: "pk_news_source_post_questions",
                table: "news_source_post_questions");

            migrationBuilder.RenameTable(
                name: "news_source_post_questions",
                newName: "article_questions");

            migrationBuilder.AddPrimaryKey(
                name: "pk_article_questions",
                table: "article_questions",
                column: "id");

            migrationBuilder.CreateTable(
                name: "article_answers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    news_link = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "unknown_news_source_notification_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    unknown_news_source_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unknown_news_source_notification_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_unknown_news_source_notification_requests_unknown_news_sour",
                        column: x => x.unknown_news_source_id,
                        principalTable: "unknown_news_sources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_article_answers_user_id",
                table: "article_answers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_unknown_news_source_notification_requests_unknown_news_sour",
                table: "unknown_news_source_notification_requests",
                column: "unknown_news_source_id");
        }
    }
}
