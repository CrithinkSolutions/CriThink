using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[] { new Guid("4c28eed7-a34a-4534-9c2c-5ffe86b72393"), "4E597EE6-5339-44B0-988E-258AD486BE49", "FreeUser", "FREEUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: new Guid("4c28eed7-a34a-4534-9c2c-5ffe86b72393"));
        }
    }
}
