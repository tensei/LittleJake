using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LittleSteve.Data.Migrations
{
    public partial class AddedYoutuberAndAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YoutuberId",
                table: "GuildOwners",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Youtubers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LatestVideoDate = table.Column<DateTimeOffset>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Youtubers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeAlertSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DiscordChannelId = table.Column<long>(nullable: false),
                    MessageId = table.Column<long>(nullable: false),
                    YoutuberId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeAlertSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeAlertSubscriptions_Youtubers_YoutuberId",
                        column: x => x.YoutuberId,
                        principalTable: "Youtubers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildOwners_YoutuberId",
                table: "GuildOwners",
                column: "YoutuberId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeAlertSubscriptions_YoutuberId",
                table: "YoutubeAlertSubscriptions",
                column: "YoutuberId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildOwners_Youtubers_YoutuberId",
                table: "GuildOwners",
                column: "YoutuberId",
                principalTable: "Youtubers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildOwners_Youtubers_YoutuberId",
                table: "GuildOwners");

            migrationBuilder.DropTable(
                name: "YoutubeAlertSubscriptions");

            migrationBuilder.DropTable(
                name: "Youtubers");

            migrationBuilder.DropIndex(
                name: "IX_GuildOwners_YoutuberId",
                table: "GuildOwners");

            migrationBuilder.DropColumn(
                name: "YoutuberId",
                table: "GuildOwners");
        }
    }
}
