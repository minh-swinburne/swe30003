using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTransactionTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time",
                table: "payments");

            migrationBuilder.AddColumn<DateTime>(
                name: "transaction_time",
                table: "payments",
                type: "DATETIME",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transaction_time",
                table: "payments");

            migrationBuilder.AddColumn<DateTime>(
                name: "time",
                table: "payments",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
