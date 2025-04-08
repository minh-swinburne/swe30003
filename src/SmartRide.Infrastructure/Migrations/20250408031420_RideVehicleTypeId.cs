using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RideVehicleTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "vehicle_type",
                table: "rides",
                newName: "vehicle_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_rides_vehicle_type_id",
                table: "rides",
                column: "vehicle_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_rides_vehicle_types_vehicle_type_id",
                table: "rides",
                column: "vehicle_type_id",
                principalTable: "vehicle_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rides_vehicle_types_vehicle_type_id",
                table: "rides");

            migrationBuilder.DropIndex(
                name: "IX_rides_vehicle_type_id",
                table: "rides");

            migrationBuilder.RenameColumn(
                name: "vehicle_type_id",
                table: "rides",
                newName: "vehicle_type");
        }
    }
}
