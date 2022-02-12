using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class SearchedNewsSequenceIncrementByUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "searched_news",
                type: "bigint",
                nullable: false,
                defaultValueSql: "nextval('\"sequence_searchednews\"')",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterSequence(
                name: "sequence_searchednews",
                oldIncrementBy: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "searched_news",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "nextval('\"sequence_searchednews\"')");

            migrationBuilder.AlterSequence(
                name: "sequence_searchednews",
                incrementBy: 10);
        }
    }
}
