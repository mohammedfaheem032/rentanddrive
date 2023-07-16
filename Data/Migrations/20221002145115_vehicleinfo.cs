using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentNDrive.Data.Migrations
{
    public partial class vehicleinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleManufacturer = table.Column<int>(type: "int", nullable: false),
                    VehiclePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceType = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Doors = table.Column<int>(type: "int", nullable: false),
                    Passengers = table.Column<int>(type: "int", nullable: false),
                    Luggage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirConditioning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Terms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleAvailableStatus = table.Column<bool>(type: "bit", nullable: false),
                    FeatureVehicle = table.Column<bool>(type: "bit", nullable: false),
                    VehicleImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleActiveStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleInfo");
        }
    }
}
