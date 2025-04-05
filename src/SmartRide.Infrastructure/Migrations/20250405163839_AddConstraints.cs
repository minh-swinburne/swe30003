using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_feedbacks_rides_ride_id",
                table: "feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_feedbacks_ride_id",
                table: "feedbacks");

            migrationBuilder.RenameColumn(
                name: "payment_time",
                table: "payments",
                newName: "time");

            migrationBuilder.RenameColumn(
                name: "license_number",
                table: "licenses",
                newName: "number");

            migrationBuilder.CreateIndex(
                name: "IX_licenses_number",
                table: "licenses",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_ride_id",
                table: "feedbacks",
                column: "ride_id",
                unique: true);

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_feedbacks_rides_ride_id",
                table: "feedbacks",
                column: "ride_id",
                principalTable: "rides",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_licenses_number",
                table: "licenses");

            migrationBuilder.DropIndex(
                name: "IX_feedbacks_ride_id",
                table: "feedbacks");

            migrationBuilder.RenameColumn(
                name: "time",
                table: "payments",
                newName: "payment_time");

            migrationBuilder.RenameColumn(
                name: "number",
                table: "licenses",
                newName: "license_number");

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_ride_id",
                table: "feedbacks",
                column: "ride_id");
        }
    }
}
