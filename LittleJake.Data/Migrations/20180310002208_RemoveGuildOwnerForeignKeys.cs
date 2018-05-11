using Microsoft.EntityFrameworkCore.Migrations;

namespace LittleJake.Data.Migrations
{
    public partial class RemoveGuildOwnerForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                "GuildOwners");

            migrationBuilder.DropForeignKey(
                "FK_GuildOwners_TwitterUsers_TwitterUserId",
                "GuildOwners");

            migrationBuilder.DropForeignKey(
                "FK_GuildOwners_Youtubers_YoutuberId",
                "GuildOwners");

            migrationBuilder.DropIndex(
                "IX_GuildOwners_TwitchStreamerId",
                "GuildOwners");

            migrationBuilder.DropIndex(
                "IX_GuildOwners_YoutuberId",
                "GuildOwners");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                "IX_GuildOwners_TwitchStreamerId",
                "GuildOwners",
                "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                "IX_GuildOwners_YoutuberId",
                "GuildOwners",
                "YoutuberId");

            migrationBuilder.AddForeignKey(
                "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                "GuildOwners",
                "TwitchStreamerId",
                "TwitchStreamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_GuildOwners_TwitterUsers_TwitterUserId",
                "GuildOwners",
                "TwitterUserId",
                "TwitterUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_GuildOwners_Youtubers_YoutuberId",
                "GuildOwners",
                "YoutuberId",
                "Youtubers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}