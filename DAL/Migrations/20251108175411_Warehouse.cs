using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Warehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CarAndMatrerialWeight",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CarDriverName",
                table: "Transactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CarWeight",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageOfImpurities",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightOfImpurities",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "MaterialTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialTypes_Warehouses_WarehouseId",
                table: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_MaterialTypes_WarehouseId",
                table: "MaterialTypes");

            migrationBuilder.DropColumn(
                name: "CarAndMatrerialWeight",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CarDriverName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CarWeight",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PercentageOfImpurities",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WeightOfImpurities",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "MaterialTypes");
        }
    }
}
