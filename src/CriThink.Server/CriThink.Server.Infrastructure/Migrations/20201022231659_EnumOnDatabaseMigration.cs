using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class EnumOnDatabaseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Authenticity",
                table: "NewsSourceCategories",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Authenticity",
                table: "NewsSourceCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
