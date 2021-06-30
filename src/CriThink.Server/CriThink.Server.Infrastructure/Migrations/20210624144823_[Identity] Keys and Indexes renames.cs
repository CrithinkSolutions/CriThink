using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class IdentityKeysandIndexesrenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_role_claims_asp_net_roles_user_role_id",
                table: "aspnet_role_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_user_claims_asp_net_users_user_id",
                table: "aspnet_user_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_user_logins_asp_net_users_user_id",
                table: "aspnet_user_logins");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_asp_net_roles_user_role_id",
                table: "aspnet_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_asp_net_users_user_id",
                table: "aspnet_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_tokens_asp_net_users_user_id",
                table: "aspnet_user_tokens");

            migrationBuilder.DropForeignKey(
                name: "fk_debunking_news_publishers_debunking_news_countries_country_",
                table: "debunking_news_publishers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_tokens",
                table: "aspnet_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "aspnet_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_logins",
                table: "aspnet_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_claims",
                table: "aspnet_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_claims",
                table: "aspnet_role_claims");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_role_id",
                table: "aspnet_user_roles",
                newName: "ix_aspnet_user_roles_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_logins_user_id",
                table: "aspnet_user_logins",
                newName: "ix_aspnet_user_logins_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_claims_user_id",
                table: "aspnet_user_claims",
                newName: "ix_aspnet_user_claims_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_claims_role_id",
                table: "aspnet_role_claims",
                newName: "ix_aspnet_role_claims_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_aspnet_user_tokens",
                table: "aspnet_user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_aspnet_user_roles",
                table: "aspnet_user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_aspnet_user_logins",
                table: "aspnet_user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_aspnet_user_claims",
                table: "aspnet_user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_aspnet_role_claims",
                table: "aspnet_role_claims",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_aspnet_role_claims_asp_net_roles_role_id",
                table: "aspnet_role_claims",
                column: "role_id",
                principalTable: "user_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aspnet_user_claims_asp_net_users_user_id",
                table: "aspnet_user_claims",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aspnet_user_logins_asp_net_users_user_id",
                table: "aspnet_user_logins",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aspnet_user_roles_asp_net_roles_role_id",
                table: "aspnet_user_roles",
                column: "role_id",
                principalTable: "user_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aspnet_user_roles_asp_net_users_user_id",
                table: "aspnet_user_roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aspnet_user_tokens_asp_net_users_user_id",
                table: "aspnet_user_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_debunking_news_publishers_debunking_news_countries_country_id",
                table: "debunking_news_publishers",
                column: "country_id",
                principalTable: "debunking_news_countries",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_aspnet_role_claims_asp_net_roles_role_id",
                table: "aspnet_role_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_aspnet_user_claims_asp_net_users_user_id",
                table: "aspnet_user_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_aspnet_user_logins_asp_net_users_user_id",
                table: "aspnet_user_logins");

            migrationBuilder.DropForeignKey(
                name: "fk_aspnet_user_roles_asp_net_roles_role_id",
                table: "aspnet_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_aspnet_user_roles_asp_net_users_user_id",
                table: "aspnet_user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_aspnet_user_tokens_asp_net_users_user_id",
                table: "aspnet_user_tokens");

            migrationBuilder.DropForeignKey(
                name: "fk_debunking_news_publishers_debunking_news_countries_country_id",
                table: "debunking_news_publishers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_aspnet_user_tokens",
                table: "aspnet_user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_aspnet_user_roles",
                table: "aspnet_user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_aspnet_user_logins",
                table: "aspnet_user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_aspnet_user_claims",
                table: "aspnet_user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_aspnet_role_claims",
                table: "aspnet_role_claims");

            migrationBuilder.RenameIndex(
                name: "ix_aspnet_user_roles_role_id",
                table: "aspnet_user_roles",
                newName: "ix_user_roles_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_aspnet_user_logins_user_id",
                table: "aspnet_user_logins",
                newName: "ix_user_logins_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_aspnet_user_claims_user_id",
                table: "aspnet_user_claims",
                newName: "ix_user_claims_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_aspnet_role_claims_role_id",
                table: "aspnet_role_claims",
                newName: "ix_role_claims_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                table: "user_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_tokens",
                table: "aspnet_user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "aspnet_user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_logins",
                table: "aspnet_user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_claims",
                table: "aspnet_user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_claims",
                table: "aspnet_role_claims",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_role_claims_asp_net_roles_user_role_id",
                table: "aspnet_role_claims",
                column: "role_id",
                principalTable: "user_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_claims_asp_net_users_user_id",
                table: "aspnet_user_claims",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_logins_asp_net_users_user_id",
                table: "aspnet_user_logins",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_asp_net_roles_user_role_id",
                table: "aspnet_user_roles",
                column: "role_id",
                principalTable: "user_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_asp_net_users_user_id",
                table: "aspnet_user_roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_tokens_asp_net_users_user_id",
                table: "aspnet_user_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_debunking_news_publishers_debunking_news_countries_country_",
                table: "debunking_news_publishers",
                column: "country_id",
                principalTable: "debunking_news_countries",
                principalColumn: "id");
        }
    }
}
