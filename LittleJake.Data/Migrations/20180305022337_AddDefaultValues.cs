﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LittleJake.Data.Migrations
{
    public partial class AddDefaultValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                "LatestVideoDate",
                "Youtubers",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "StreamEndTime",
                "TwitchStreamers",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "SteamStartTime",
                "TwitchStreamers",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                "LatestVideoDate",
                "Youtubers",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldDefaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "StreamEndTime",
                "TwitchStreamers",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldDefaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                "SteamStartTime",
                "TwitchStreamers",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldDefaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}