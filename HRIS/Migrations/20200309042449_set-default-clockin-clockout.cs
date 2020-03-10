using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRIS.Migrations
{
    public partial class setdefaultclockinclockout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "Attendance");

            migrationBuilder.AddColumn<string>(
                name: "ClockInStatus",
                table: "Attendance",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClockOut",
                table: "Attendance",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ClockOutStatus",
                table: "Attendance",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClockInStatus",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "ClockOut",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "ClockOutStatus",
                table: "Attendance");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "Attendance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
