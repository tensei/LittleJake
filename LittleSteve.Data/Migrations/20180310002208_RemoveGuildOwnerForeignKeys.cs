using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LittleSteve.Data.Migrations
{
    public partial class RemoveGuildOwnerForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                table: "GuildOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildOwners_TwitterUsers_TwitterUserId",
                table: "GuildOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildOwners_Youtubers_YoutuberId",
                table: "GuildOwners");

            migrationBuilder.DropIndex(
                name: "IX_GuildOwners_TwitchStreamerId",
                table: "GuildOwners");

            migrationBuilder.DropIndex(
                name: "IX_GuildOwners_YoutuberId",
                table: "GuildOwners");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GuildOwners_TwitchStreamerId",
                table: "GuildOwners",
                column: "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildOwners_YoutuberId",
                table: "GuildOwners",
                column: "YoutuberId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                table: "GuildOwners",
                column: "TwitchStreamerId",
                principalTable: "TwitchStreamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildOwners_TwitterUsers_TwitterUserId",
                table: "GuildOwners",
                column: "TwitterUserId",
                principalTable: "TwitterUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildOwners_Youtubers_YoutuberId",
                table: "GuildOwners",
                column: "YoutuberId",
                principalTable: "Youtubers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
