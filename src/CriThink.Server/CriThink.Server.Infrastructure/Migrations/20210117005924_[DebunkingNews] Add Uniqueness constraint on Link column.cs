using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class DebunkingNewsAddUniquenessconstraintonLinkcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "ix_debunking_news_link",
                table: "debunking_news",
                column: "link",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_debunking_news_link",
                table: "debunking_news");

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                column: "concurrency_stamp",
                value: "3855861b-a901-450e-978c-482ee7659a83");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                column: "concurrency_stamp",
                value: "30b63915-ed37-418d-8982-22e80354b267");
        }
    }
}
