using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class driverAndMetalType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverExpense_Driver_DriverId",
                table: "DriverExpense");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverExpense_Travel_TravelId",
                table: "DriverExpense");

            migrationBuilder.DropIndex(
                name: "IX_DriverExpense_TravelId",
                table: "DriverExpense");

            migrationBuilder.DropColumn(
                name: "DistanceKm",
                table: "Travel");

            migrationBuilder.DropColumn(
                name: "ExpenseType",
                table: "DriverExpense");

            migrationBuilder.DropColumn(
                name: "TravelId",
                table: "DriverExpense");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Travel",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Travel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MaterialTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "DriverExpense",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MoneyBalance",
                table: "Driver",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverExpense_Driver_DriverId",
                table: "DriverExpense",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverExpense_Driver_DriverId",
                table: "DriverExpense");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Travel");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Travel");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MaterialTypes");

            migrationBuilder.DropColumn(
                name: "MoneyBalance",
                table: "Driver");

            migrationBuilder.AddColumn<double>(
                name: "DistanceKm",
                table: "Travel",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "DriverExpense",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseType",
                table: "DriverExpense",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TravelId",
                table: "DriverExpense",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverExpense_TravelId",
                table: "DriverExpense",
                column: "TravelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverExpense_Driver_DriverId",
                table: "DriverExpense",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverExpense_Travel_TravelId",
                table: "DriverExpense",
                column: "TravelId",
                principalTable: "Travel",
                principalColumn: "Id");
        }
    }
}
