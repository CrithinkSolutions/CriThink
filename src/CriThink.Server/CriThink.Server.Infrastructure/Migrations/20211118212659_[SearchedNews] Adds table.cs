using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class SearchedNewsAddstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authenticity",
                table: "user_searches");

            migrationBuilder.DropColumn(
                name: "news_link",
                table: "user_searches");

            migrationBuilder.DropColumn(
                name: "rate",
                table: "user_searches");

            migrationBuilder.CreateSequence(
                name: "sequence_searchednews",
                incrementBy: 10);

            migrationBuilder.AddColumn<long>(
                name: "searched_news_id",
                table: "user_searches",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "searched_text",
                table: "user_searches",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "searched_news",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    link = table.Column<string>(type: "text", nullable: false),
                    keywords = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    fav_icon = table.Column<string>(type: "text", nullable: true),
                    rate = table.Column<decimal>(type: "numeric", nullable: true),
                    authenticity = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_searched_news", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_searches_searched_news_id",
                table: "user_searches",
                column: "searched_news_id");

            migrationBuilder.CreateIndex(
                name: "ix_searched_news_link",
                table: "searched_news",
                column: "link");

            migrationBuilder.AddForeignKey(
                name: "fk_user_searches_searched_news_searched_news_id",
                table: "user_searches",
                column: "searched_news_id",
                principalTable: "searched_news",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_searches_searched_news_searched_news_id",
                table: "user_searches");

            migrationBuilder.DropTable(
                name: "searched_news");

            migrationBuilder.DropIndex(
                name: "ix_user_searches_searched_news_id",
                table: "user_searches");

            migrationBuilder.DropSequence(
                name: "sequence_searchednews");

            migrationBuilder.DropColumn(
                name: "searched_news_id",
                table: "user_searches");

            migrationBuilder.DropColumn(
                name: "searched_text",
                table: "user_searches");

            migrationBuilder.AddColumn<string>(
                name: "authenticity",
                table: "user_searches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "news_link",
                table: "user_searches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "rate",
                table: "user_searches",
                type: "numeric",
                nullable: true);
        }
    }
}
