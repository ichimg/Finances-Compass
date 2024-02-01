using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddExchangeRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EurExchangeRate",
                table: "Debts",
                type: "decimal(18,4)",
                nullable: true,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UsdExchangeRate",
                table: "Debts",
                type: "decimal(18,4)",
                nullable: true,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EurExchangeRate",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "UsdExchangeRate",
                table: "Debts");
        }
    }
}
