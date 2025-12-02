using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MerchantFinanceEdit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StartLocation",
                table: "Travel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Travel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Travel_DriverId",
                table: "Travel",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Travel_Driver_DriverId",
                table: "Travel",
                column: "DriverId",
                principalTable: "Driver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Travel_Driver_DriverId",
                table: "Travel");

            migrationBuilder.DropIndex(
                name: "IX_Travel_DriverId",
                table: "Travel");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Travel");

            migrationBuilder.AlterColumn<string>(
                name: "StartLocation",
                table: "Travel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
