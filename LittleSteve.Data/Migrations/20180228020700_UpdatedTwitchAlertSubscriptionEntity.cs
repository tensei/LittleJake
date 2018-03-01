using Microsoft.EntityFrameworkCore.Migrations;

namespace LittleSteve.Data.Migrations
{
    public partial class UpdatedTwitchAlertSubscriptionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                "MessageId",
                "TwitchAlertSubscriptions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                "ShouldPin",
                "TwitchAlertSubscriptions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "MessageId",
                "TwitchAlertSubscriptions");

            migrationBuilder.DropColumn(
                "ShouldPin",
                "TwitchAlertSubscriptions");
        }
    }
}