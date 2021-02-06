using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class _202002060235_UnknownSourcesUnknownSourceNotificationRequestsCreatesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "unknown_source_notification_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    requested_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    unknown_news_source_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unknown_source_notification_requests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unknown_sources",
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
                    table.PrimaryKey("pk_unknown_sources", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                column: "concurrency_stamp",
                value: "4ff401ad-41f0-4ff3-ac9a-642e9cd76a38");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                column: "concurrency_stamp",
                value: "fd57f1bc-a1d3-4a40-97f5-03def2bcc800");

            migrationBuilder.CreateIndex(
                name: "ix_unknown_sources_uri",
                table: "unknown_sources",
                column: "uri",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "unknown_source_notification_requests");

            migrationBuilder.DropTable(
                name: "unknown_sources");

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
