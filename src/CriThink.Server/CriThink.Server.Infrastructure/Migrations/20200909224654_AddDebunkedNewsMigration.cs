using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class AddDebunkedNewsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DebunkingNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    PublishingDate = table.Column<DateTime>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    NewsCaption = table.Column<string>(maxLength: 500, nullable: true),
                    PublisherName = table.Column<string>(nullable: true),
                    Keywords = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebunkedNews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DebunkingNewsTriggerLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsSuccessful = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebunkingNewsTriggerLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebunkingNews");

            migrationBuilder.DropTable(
                name: "DebunkingNewsTriggerLogs");
        }
    }
}
