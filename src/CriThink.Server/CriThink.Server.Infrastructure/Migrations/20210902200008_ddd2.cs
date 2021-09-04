using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class ddd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "time_stamp",
                table: "debunking_news_trigger_logs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_stamp",
                table: "debunking_news_trigger_logs");
        }
    }
}
