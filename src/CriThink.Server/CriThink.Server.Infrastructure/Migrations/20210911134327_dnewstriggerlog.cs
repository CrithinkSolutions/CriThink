using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class dnewstriggerlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_successful",
                table: "debunking_news_trigger_logs");

            migrationBuilder.RenameColumn(
                name: "fail_reason",
                table: "debunking_news_trigger_logs",
                newName: "failures");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "debunking_news_trigger_logs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "debunking_news_trigger_logs");

            migrationBuilder.RenameColumn(
                name: "failures",
                table: "debunking_news_trigger_logs",
                newName: "fail_reason");

            migrationBuilder.AddColumn<bool>(
                name: "is_successful",
                table: "debunking_news_trigger_logs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
