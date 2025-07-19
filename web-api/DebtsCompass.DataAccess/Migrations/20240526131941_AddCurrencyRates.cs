using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EurExchangeRate",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "UsdExchangeRate",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "EurExchangeRate",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "UsdExchangeRate",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "EurExchangeRate",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "UsdExchangeRate",
                table: "Debts");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyRateId",
                table: "Incomes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyRateId",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyRateId",
                table: "Debts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EurExchangeRate = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UsdExchangeRate = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_CurrencyRateId",
                table: "Incomes",
                column: "CurrencyRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CurrencyRateId",
                table: "Expenses",
                column: "CurrencyRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Debts_CurrencyRateId",
                table: "Debts",
                column: "CurrencyRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_CurrencyRates_CurrencyRateId",
                table: "Debts",
                column: "CurrencyRateId",
                principalTable: "CurrencyRates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_CurrencyRates_CurrencyRateId",
                table: "Expenses",
                column: "CurrencyRateId",
                principalTable: "CurrencyRates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_CurrencyRates_CurrencyRateId",
                table: "Incomes",
                column: "CurrencyRateId",
                principalTable: "CurrencyRates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_CurrencyRates_CurrencyRateId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_CurrencyRates_CurrencyRateId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_CurrencyRates_CurrencyRateId",
                table: "Incomes");

            migrationBuilder.DropTable(
                name: "CurrencyRates");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_CurrencyRateId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_CurrencyRateId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Debts_CurrencyRateId",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "CurrencyRateId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "CurrencyRateId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "CurrencyRateId",
                table: "Debts");

            migrationBuilder.AddColumn<decimal>(
                name: "EurExchangeRate",
                table: "Incomes",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UsdExchangeRate",
                table: "Incomes",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EurExchangeRate",
                table: "Expenses",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UsdExchangeRate",
                table: "Expenses",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EurExchangeRate",
                table: "Debts",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UsdExchangeRate",
                table: "Debts",
                type: "decimal(18,4)",
                nullable: true);
        }
    }
}
