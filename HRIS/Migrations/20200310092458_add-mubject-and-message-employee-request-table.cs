using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Migrations
{
    public partial class addmubjectandmessageemployeerequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "EmployeeRequest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "EmployeeRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "EmployeeRequest");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "EmployeeRequest");
        }
    }
}
