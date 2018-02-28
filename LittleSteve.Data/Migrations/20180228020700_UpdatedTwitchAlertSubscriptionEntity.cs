using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LittleSteve.Data.Migrations
{
    public partial class UpdatedTwitchAlertSubscriptionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MessageId",
                table: "TwitchAlertSubscriptions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldPin",
                table: "TwitchAlertSubscriptions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "TwitchAlertSubscriptions");

            migrationBuilder.DropColumn(
                name: "ShouldPin",
                table: "TwitchAlertSubscriptions");
        }
    }
}
