using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTrack.VehicleLocationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialLocationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    TimeStampUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLocations_TimeStampUtc",
                table: "VehicleLocations",
                column: "TimeStampUtc");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLocations_VehicleCode",
                table: "VehicleLocations",
                column: "VehicleCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleLocations");
        }
    }
}
