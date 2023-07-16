using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentNDrive.Data.Migrations
{
    public partial class googlemap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VLatitude",
                table: "VehicleInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VLongitude",
                table: "VehicleInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VLatitude",
                table: "VehicleInfo");

            migrationBuilder.DropColumn(
                name: "VLongitude",
                table: "VehicleInfo");
        }
    }
}
