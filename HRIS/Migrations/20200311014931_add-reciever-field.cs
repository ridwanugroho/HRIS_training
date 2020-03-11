using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Migrations
{
    public partial class addrecieverfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotificationLog");

            migrationBuilder.AddColumn<string>(
                name: "Reciever",
                table: "NotificationLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "NotificationLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reciever",
                table: "NotificationLog");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "NotificationLog");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "NotificationLog",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
