using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class UserAddRegisteredOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "registered_on",
                table: "users",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "registered_on",
                table: "users");
        }
    }
}
