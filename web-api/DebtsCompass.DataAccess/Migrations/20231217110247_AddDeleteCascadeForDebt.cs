using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteCascadeForDebt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_Debts_DebtId",
                table: "DebtAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_Debts_DebtId",
                table: "DebtAssignments",
                column: "DebtId",
                principalTable: "Debts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_Debts_DebtId",
                table: "DebtAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_Debts_DebtId",
                table: "DebtAssignments",
                column: "DebtId",
                principalTable: "Debts",
                principalColumn: "Id");
        }
    }
}
