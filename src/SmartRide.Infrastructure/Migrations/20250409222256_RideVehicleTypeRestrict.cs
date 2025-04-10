using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RideVehicleTypeRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rides_users_passenger_id",
                table: "rides");

            migrationBuilder.DropForeignKey(
                name: "FK_rides_vehicle_types_vehicle_type_id",
                table: "rides");

            migrationBuilder.AddForeignKey(
                name: "FK_rides_users_passenger_id",
                table: "rides",
                column: "passenger_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rides_vehicle_types_vehicle_type_id",
                table: "rides",
                column: "vehicle_type_id",
                principalTable: "vehicle_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rides_users_passenger_id",
                table: "rides");

            migrationBuilder.DropForeignKey(
                name: "FK_rides_vehicle_types_vehicle_type_id",
                table: "rides");

            migrationBuilder.AddForeignKey(
                name: "FK_rides_users_passenger_id",
                table: "rides",
                column: "passenger_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rides_vehicle_types_vehicle_type_id",
                table: "rides",
                column: "vehicle_type_id",
                principalTable: "vehicle_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
