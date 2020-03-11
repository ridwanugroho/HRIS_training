using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Migrations
{
    public partial class addnotifIdtorequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotifId",
                table: "EmployeeRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifId",
                table: "EmployeeRequest");
        }
    }
}
