using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class NewsSourceCategoriesLocalizationsupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"),
                column: "description",
                value: "NewsSourceConspiracist");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("3cc36977-5115-45ea-88e9-7c86f19b6cd6"),
                column: "description",
                value: "NewsSourceSuspicious");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("6254c650-76f0-4bba-ba5e-01218891f729"),
                column: "description",
                value: "NewsSourceReliable");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("762f6747-e0de-4b3b-80b1-46d599ce02df"),
                column: "description",
                value: "NewsSourceSatirical");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("d66bf55d-d30d-448f-be69-d2e0cebdd26a"),
                column: "description",
                value: "NewsSourceSocialMedia");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"),
                column: "description",
                value: "NewsSourceFakeNews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("28e0a6fc-4937-4ed3-949a-75bec8f8d2e1"),
                column: "description",
                value: "Sources in the Conspiracy category may publish unverifiable information that is not always supported by evidence. Actually, they usually publish Conspiracy theories consisting in explanations for an event or situation that invoke a conspiracy by sinister and powerful groups, often political in motivation. These sources may be untrustworthy for credible/verifiable information, therefore fact checking and further investigation is recommended.");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("3cc36977-5115-45ea-88e9-7c86f19b6cd6"),
                column: "description",
                value: "These sources may publish a mix of non-factual and factual information which is misrepresented and/or reported with bias (e.g. political leaning bias). Therefore, information coming from these sources should be double-checked considering reliable sources of news.");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("6254c650-76f0-4bba-ba5e-01218891f729"),
                column: "description",
                value: "These sources have minimal bias and use very few loaded words (wording that attempts to influence an audience by using appeal to emotion or stereotypes).  The reporting is factual and usually sourced.");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("762f6747-e0de-4b3b-80b1-46d599ce02df"),
                column: "description",
                value: "These sources exclusively use humor, irony, exaggeration, or ridicule to expose and criticize people’s stupidity or vices, particularly in the context of contemporary politics and other topical issues. Primarily these sources are clear that they are satire and do not attempt to deceive.");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("d66bf55d-d30d-448f-be69-d2e0cebdd26a"),
                column: "description",
                value: "Social media platforms (such as Facebook, Twitter, Reddit and so on) cannot be classified strictly as sources of information. Rather these platforms are just mediums through which information can be shared. In these cases, verifying the reliability of the user/page sharing the information is recommended.");

            migrationBuilder.UpdateData(
                table: "news_source_categories",
                keyColumn: "id",
                keyValue: new Guid("ec5eb3b0-1a32-4698-88e2-9ae81ec87176"),
                column: "description",
                value: "Sources in this category show extreme bias, poor or no sourcing to credible information, a complete lack of transparency and/or publish fake news for profit or ideologically influence the audience. These sources may be very untrustworthy and should be fact checked.");
        }
    }
}
