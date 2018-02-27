using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LittleSteve.Data.Migrations
{
    public partial class AddedTwitchEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitterAlerts");

            migrationBuilder.AddColumn<long>(
                name: "TwitchStreamerId",
                table: "GuildOwners",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "TwitchStreamers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    SteamStartTime = table.Column<DateTimeOffset>(nullable: false),
                    StreamEndTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchStreamers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterAlertSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    TwitterUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterAlertSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitterAlertSubscriptions_TwitterUsers_TwitterUserId",
                        column: x => x.TwitterUserId,
                        principalTable: "TwitterUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitchAlertSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    TwitchStreamerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchAlertSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchAlertSubscriptions_TwitchStreamers_TwitchStreamerId",
                        column: x => x.TwitchStreamerId,
                        principalTable: "TwitchStreamers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildOwners_TwitchStreamerId",
                table: "GuildOwners",
                column: "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchAlertSubscriptions_TwitchStreamerId",
                table: "TwitchAlertSubscriptions",
                column: "TwitchStreamerId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterAlertSubscriptions_TwitterUserId",
                table: "TwitterAlertSubscriptions",
                column: "TwitterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                table: "GuildOwners",
                column: "TwitchStreamerId",
                principalTable: "TwitchStreamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildOwners_TwitchStreamers_TwitchStreamerId",
                table: "GuildOwners");

            migrationBuilder.DropTable(
                name: "TwitchAlertSubscriptions");

            migrationBuilder.DropTable(
                name: "TwitterAlertSubscriptions");

            migrationBuilder.DropTable(
                name: "TwitchStreamers");

            migrationBuilder.DropIndex(
                name: "IX_GuildOwners_TwitchStreamerId",
                table: "GuildOwners");

            migrationBuilder.DropColumn(
                name: "TwitchStreamerId",
                table: "GuildOwners");

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
                name: "IX_TwitterAlerts_TwitterUserId",
                table: "TwitterAlerts",
                column: "TwitterUserId");
        }
    }
}
