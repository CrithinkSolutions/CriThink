using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class UnknownSourcesUnknownSourceNotificationRequestsCreatesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "unknown_news_sources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    uri = table.Column<string>(type: "text", nullable: false),
                    first_requested_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    identified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    request_count = table.Column<int>(type: "integer", nullable: false),
                    authenticity = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unknown_news_sources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unknown_news_source_notification_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    requested_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                column: "concurrency_stamp",
                value: "43ec0e6f-4239-4e39-892f-4110060d16fa");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                column: "concurrency_stamp",
                value: "e33be67f-e762-4cc5-b8a3-316d5681dbc8");

            migrationBuilder.CreateIndex(
                name: "ix_unknown_news_source_notification_requests_unknown_news_sour",
                table: "unknown_news_source_notification_requests",
                column: "unknown_news_source_id");

            migrationBuilder.CreateIndex(
                name: "ix_unknown_news_sources_uri",
                table: "unknown_news_sources",
                column: "uri",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "unknown_news_source_notification_requests");

            migrationBuilder.DropTable(
                name: "unknown_news_sources");

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                column: "concurrency_stamp",
                value: "19682149-72c5-4587-923e-380d5c11d05d");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                column: "concurrency_stamp",
                value: "9ad64c83-504c-43ed-8b72-bc85bb851e7d");
        }
    }
}
