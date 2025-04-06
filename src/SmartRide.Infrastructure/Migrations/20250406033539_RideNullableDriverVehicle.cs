using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RideNullableDriverVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rides_users_driver_id",
                table: "rides");

            migrationBuilder.DropForeignKey(
                name: "FK_rides_vehicles_vehicle_id",
                table: "rides");

            migrationBuilder.AlterColumn<byte[]>(
                name: "vehicle_id",
                table: "rides",
                type: "BINARY(16)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BINARY(16)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "driver_id",
                table: "rides",
                type: "BINARY(16)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BINARY(16)");

            migrationBuilder.AddForeignKey(
                name: "FK_rides_users_driver_id",
                table: "rides",
                column: "driver_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rides_vehicles_vehicle_id",
                table: "rides",
                column: "vehicle_id",
                principalTable: "vehicles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rides_users_driver_id",
                table: "rides");

            migrationBuilder.DropForeignKey(
                name: "FK_rides_vehicles_vehicle_id",
                table: "rides");

            migrationBuilder.AlterColumn<byte[]>(
                name: "vehicle_id",
                table: "rides",
                type: "BINARY(16)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BINARY(16)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "driver_id",
                table: "rides",
                type: "BINARY(16)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BINARY(16)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_rides_users_driver_id",
                table: "rides",
                column: "driver_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rides_vehicles_vehicle_id",
                table: "rides",
                column: "vehicle_id",
                principalTable: "vehicles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
