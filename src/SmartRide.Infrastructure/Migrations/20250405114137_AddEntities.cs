using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartRide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locations_users_user_id",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "identity_id",
                table: "users");

            migrationBuilder.AlterColumn<decimal>(
                name: "fare",
                table: "rides",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "FLOAT");

            migrationBuilder.AddColumn<byte[]>(
                name: "user_id",
                table: "rides",
                type: "BINARY(16)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "issued_country",
                table: "licenses",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "license_number",
                table: "licenses",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    id = table.Column<byte[]>(type: "BINARY(16)", nullable: false),
                    ride_id = table.Column<byte[]>(type: "BINARY(16)", nullable: false),
                    rating = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    comment = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_time = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    updated_time = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedbacks", x => x.id);
                    table.ForeignKey(
                        name: "FK_feedbacks_rides_ride_id",
                        column: x => x.ride_id,
                        principalTable: "rides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    is_enabled = table.Column<ulong>(type: "BIT", nullable: false),
                    name = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<byte[]>(type: "BINARY(16)", nullable: false),
                    ride_id = table.Column<byte[]>(type: "BINARY(16)", nullable: false),
                    amount = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    payment_method_id = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    payment_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    status = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    created_time = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    updated_time = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_payment_methods_payment_method_id",
                        column: x => x.payment_method_id,
                        principalTable: "payment_methods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payments_rides_ride_id",
                        column: x => x.ride_id,
                        principalTable: "rides",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "payment_methods",
                columns: new[] { "id", "description", "is_enabled", "name" },
                values: new object[,]
                {
                    { (sbyte)1, "Cash payment", 1ul, "Cash" },
                    { (sbyte)2, "Credit card payment", 1ul, "CreditCard" },
                    { (sbyte)3, "PayPal payment", 1ul, "PayPal" }
                });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: (sbyte)1,
                column: "description",
                value: "User who books rides");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: (sbyte)2,
                column: "description",
                value: "User who provides rides");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: (sbyte)3,
                column: "description",
                value: "User who manages the system");

            migrationBuilder.InsertData(
                table: "vehicle_types",
                columns: new[] { "id", "capacity", "description", "name" },
                values: new object[,]
                {
                    { (sbyte)1, (sbyte)2, "Two-wheeled vehicle with only one seat", "Motorbike" },
                    { (sbyte)2, (sbyte)4, "Compact car with 4 seats", "SmallCar" },
                    { (sbyte)3, (sbyte)7, "Spacious car with 7 seats", "LargeCar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_rides_user_id",
                table: "rides",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_ride_id",
                table: "feedbacks",
                column: "ride_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_method_id",
                table: "payments",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_ride_id",
                table: "payments",
                column: "ride_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_locations_users_user_id",
                table: "locations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_rides_users_user_id",
                table: "rides",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_locations_users_user_id",
                table: "locations");

            migrationBuilder.DropForeignKey(
                name: "FK_rides_users_user_id",
                table: "rides");

            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropIndex(
                name: "IX_rides_user_id",
                table: "rides");

            migrationBuilder.DeleteData(
                table: "vehicle_types",
                keyColumn: "id",
                keyValue: (sbyte)1);

            migrationBuilder.DeleteData(
                table: "vehicle_types",
                keyColumn: "id",
                keyValue: (sbyte)2);

            migrationBuilder.DeleteData(
                table: "vehicle_types",
                keyColumn: "id",
                keyValue: (sbyte)3);

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "rides");

            migrationBuilder.DropColumn(
                name: "issued_country",
                table: "licenses");

            migrationBuilder.DropColumn(
                name: "license_number",
                table: "licenses");

            migrationBuilder.AddColumn<byte[]>(
                name: "identity_id",
                table: "users",
                type: "BINARY(16)",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "fare",
                table: "rides",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: (sbyte)1,
                column: "description",
                value: null);

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: (sbyte)2,
                column: "description",
                value: null);

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: (sbyte)3,
                column: "description",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_locations_users_user_id",
                table: "locations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
