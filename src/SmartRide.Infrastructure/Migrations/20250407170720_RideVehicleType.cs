using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RideVehicleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "rides",
                newName: "vehicle_type");

            migrationBuilder.AddColumn<sbyte>(
                name: "ride_type",
                table: "rides",
                type: "TINYINT",
                nullable: false,
                defaultValue: (sbyte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ride_type",
                table: "rides");

            migrationBuilder.RenameColumn(
                name: "vehicle_type",
                table: "rides",
                newName: "type");
        }
    }
}
