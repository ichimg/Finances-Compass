using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNewCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExpenseCategories",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("ea40f9a8-ef13-40d4-be5e-246a2dc6e5aa"), "Debts", null },
                });

            migrationBuilder.InsertData(
                table: "IncomeCategories",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("43d27aae-2fb4-479d-994a-e32c061de230"), "Debts", null },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: new Guid("ea40f9a8-ef13-40d4-be5e-246a2dc6e5aa"));

            migrationBuilder.DeleteData(
                table: "IncomeCategories",
                keyColumn: "Id",
                keyValue: new Guid("43d27aae-2fb4-479d-994a-e32c061de230"));
        }
    }
}
