using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CriThink.Server.Infrastructure.Migrations
{
    public partial class UserProfileAddstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_path",
                table: "users");

            migrationBuilder.DropColumn(
                name: "registered_on",
                table: "users");

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    given_name = table.Column<string>(type: "text", nullable: true),
                    family_name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<string>(type: "text", nullable: true),
                    avatar_path = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    telegram = table.Column<string>(type: "text", nullable: true),
                    skype = table.Column<string>(type: "text", nullable: true),
                    twitter = table.Column<string>(type: "text", nullable: true),
                    instagram = table.Column<string>(type: "text", nullable: true),
                    facebook = table.Column<string>(type: "text", nullable: true),
                    snapchat = table.Column<string>(type: "text", nullable: true),
                    youtube = table.Column<string>(type: "text", nullable: true),
                    blog = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "Date", nullable: true),
                    registered_on = table.Column<DateTime>(type: "Date", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "user_profiles",
                columns: new[] { "id", "avatar_path", "blog", "country", "date_of_birth", "description", "facebook", "family_name", "gender", "given_name", "instagram", "registered_on", "skype", "snapchat", "telegram", "twitter", "user_id", "youtube" },
                values: new object[] { new Guid("cb825a64-9cdb-48e7-8bb0-45d5bed6eee2"), null, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is the default account", null, null, null, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, new Guid("f62fc754-e296-4aca-0a3f-08d88b1daff7"), null });

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_user_id",
                table: "user_profiles",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.AddColumn<string>(
                name: "avatar_path",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "registered_on",
                table: "users",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
