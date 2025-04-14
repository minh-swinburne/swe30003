using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentCurrencyTransId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "payments",
                type: "VARCHAR(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "transaction_id",
                table: "payments",
                type: "VARCHAR(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "payments");
        }
    }
}
