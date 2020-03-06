using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Migrations
{
    public partial class refactoremployeefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "Employee",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NIK",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "_role",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "NIK",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "_role",
                table: "Employee");
        }
    }
}
