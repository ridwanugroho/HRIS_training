using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Migrations
{
    public partial class addreadstatusnotifLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReadStatus",
                table: "NotificationLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadStatus",
                table: "NotificationLog");
        }
    }
}
