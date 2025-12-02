using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MerchantFinanceEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinanceDate",
                table: "MerchantFinance",
                newName: "OperationDate");

            migrationBuilder.RenameColumn(
                name: "AmountFinance",
                table: "MerchantFinance",
                newName: "Amount");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MerchantFinance",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationDate",
                table: "MerchantFinance",
                newName: "FinanceDate");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "MerchantFinance",
                newName: "AmountFinance");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MerchantFinance",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}
