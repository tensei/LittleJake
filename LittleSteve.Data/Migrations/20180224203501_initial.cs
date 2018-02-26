using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LittleSteve.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwitterUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LastTweetId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ScreenName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildOwners",
                columns: table => new
                {
                    DiscordId = table.Column<long>(nullable: false),
                    GuildId = table.Column<long>(nullable: false),
                    TwitterUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildOwners", x => new { x.DiscordId, x.GuildId });
                    table.ForeignKey(
                        name: "FK_GuildOwners_TwitterUsers_TwitterUserId",
                        column: x => x.TwitterUserId,
                        principalTable: "TwitterUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitterAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    TwitterUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitterAlerts_TwitterUsers_TwitterUserId",
                        column: x => x.TwitterUserId,
                        principalTable: "TwitterUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildOwners_TwitterUserId",
                table: "GuildOwners",
                column: "TwitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterAlerts_TwitterUserId",
                table: "TwitterAlerts",
                column: "TwitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterUsers_LastTweetId",
                table: "TwitterUsers",
                column: "LastTweetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildOwners");

            migrationBuilder.DropTable(
                name: "TwitterAlerts");

            migrationBuilder.DropTable(
                name: "TwitterUsers");
        }
    }
}
