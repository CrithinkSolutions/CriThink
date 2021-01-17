using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class DebunkingNewsAddImageLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_link",
                table: "debunking_news",
                type: "text",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_link",
                table: "debunking_news");

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("ec1405d9-5e55-401a-b469-37a44ecd211f"),
                column: "concurrency_stamp",
                value: "1d5179e2-77f1-42bc-a09c-1feb44eb7112");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"),
                column: "concurrency_stamp",
                value: "40695ec9-2b42-4f94-ad0e-2dc1cb6436e5");
        }
    }
}
