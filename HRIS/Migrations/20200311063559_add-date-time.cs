using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Migrations
{
    public partial class adddatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "_dueDate",
                table: "EmployeeRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_dueDate",
                table: "EmployeeRequest");
        }
    }
}
