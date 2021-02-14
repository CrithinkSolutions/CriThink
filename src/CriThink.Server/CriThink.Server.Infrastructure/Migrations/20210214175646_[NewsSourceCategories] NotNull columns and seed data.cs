using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class NewsSourceCategoriesNotNullcolumnsandseeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "news_source_categories",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "news_source_categories",
                columns: new[] { "id", "authenticity", "description" },
                values: new object[,]
                {
                    { new Guid("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"), "FakeNews", "Sources in this category show extreme bias, poor or no sourcing to credible information, a complete lack of transparency and/or publish fake news. Fake News is the deliberate attempt to publish hoaxes and/or disinformation for profit or ideologically influence the audience. These sources may be very untrustworthy and should be fact checked." },
                    { new Guid("762f6747-e0de-4b3b-80b1-46d599ce02df"), "Satirical", "These sources exclusively use humor, irony, exaggeration, or ridicule to expose and criticize people’s stupidity or vices, particularly in the context of contemporary politics and other topical issues. Primarily these sources are clear that they are satire and do not attempt to deceive." },
                    { new Guid("6254c650-76f0-4bba-ba5e-01218891f729"), "Reliable", "These sources have minimal bias and use very few loaded words (wording that attempts to influence an audience by using appeal to emotion or stereotypes).  The reporting is factual and usually sourced." },
                    { new Guid("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"), "Conspiracist", "Sources in the Conspiracy category may publish unverifiable information that is not always supported by evidence. Actually, they usually publish Conspiracy theories consisting in explanations for an event or situation that invoke a conspiracy by sinister and powerful groups, often political in motivation. These sources may be untrustworthy for credible/verifiable information, therefore fact checking and further investigation is recommended when obtaining information from these sources." }
                });

            migrationBuilder.CreateIndex(
                name: "ix_news_source_categories_authenticity",
                table: "news_source_categories",
                column: "authenticity",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_news_source_categories_authenticity",
                table: "news_source_categories");

            migrationBuilder.DeleteData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"));

            migrationBuilder.DeleteData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("6254c650-76f0-4bba-ba5e-01218891f729"));

            migrationBuilder.DeleteData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("762f6747-e0de-4b3b-80b1-46d599ce02df"));

            migrationBuilder.DeleteData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"));

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "news_source_categories",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);
        }
    }
}
