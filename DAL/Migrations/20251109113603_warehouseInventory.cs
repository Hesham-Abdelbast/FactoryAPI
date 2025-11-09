using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class warehouseInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialTypes_Warehouses_WarehouseId",
                table: "MaterialTypes");

            migrationBuilder.DropIndex(
                name: "IX_MaterialTypes_WarehouseId",
                table: "MaterialTypes");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "MaterialTypes");

            migrationBuilder.CreateTable(
                name: "WarehouseInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseInventory_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseInventory_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventory_MaterialTypeId",
                table: "WarehouseInventory",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseInventory_WarehouseId",
                table: "WarehouseInventory",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseInventory");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "MaterialTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypes_WarehouseId",
                table: "MaterialTypes",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialTypes_Warehouses_WarehouseId",
                table: "MaterialTypes",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
