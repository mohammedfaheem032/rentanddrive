using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentNDrive.Data.Migrations
{
    public partial class comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "CommentsInfo");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "CommentsInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "CommentsInfo");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "CommentsInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
