using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseToTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WarehouseId",
                table: "Transactions",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Warehouses_WarehouseId",
                table: "Transactions",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Warehouses_WarehouseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_WarehouseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Transactions");
        }
    }
}
