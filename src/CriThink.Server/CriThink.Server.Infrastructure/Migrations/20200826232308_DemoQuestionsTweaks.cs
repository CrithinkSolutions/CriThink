using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class DemoQuestionsTweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_DemoNews_DemoNewsId",
                table: "QuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Questions_QuestionId",
                table: "QuestionAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "QuestionAnswers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DemoNewsId",
                table: "QuestionAnswers",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "DemoNews",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "DemoNews",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_DemoNews_DemoNewsId",
                table: "QuestionAnswers",
                column: "DemoNewsId",
                principalTable: "DemoNews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Questions_QuestionId",
                table: "QuestionAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_DemoNews_DemoNewsId",
                table: "QuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Questions_QuestionId",
                table: "QuestionAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "QuestionAnswers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "DemoNewsId",
                table: "QuestionAnswers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "DemoNews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "DemoNews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_DemoNews_DemoNewsId",
                table: "QuestionAnswers",
                column: "DemoNewsId",
                principalTable: "DemoNews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Questions_QuestionId",
                table: "QuestionAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
