using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedCurrencyPreference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignment_AspNetUsers_CreatorUserId",
                table: "DebtAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignment_AspNetUsers_SelectedUserId",
                table: "DebtAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignment_Debts_DebtId",
                table: "DebtAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignment_NonUserDebtAssignment_NonUserDebtAssignmentId",
                table: "DebtAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DebtAssignment",
                table: "DebtAssignment");

            migrationBuilder.RenameTable(
                name: "DebtAssignment",
                newName: "DebtAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignment_SelectedUserId",
                table: "DebtAssignments",
                newName: "IX_DebtAssignments_SelectedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignment_NonUserDebtAssignmentId",
                table: "DebtAssignments",
                newName: "IX_DebtAssignments_NonUserDebtAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignment_DebtId",
                table: "DebtAssignments",
                newName: "IX_DebtAssignments_DebtId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignment_CreatorUserId",
                table: "DebtAssignments",
                newName: "IX_DebtAssignments_CreatorUserId");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyPreference",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DebtAssignments",
                table: "DebtAssignments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_AspNetUsers_CreatorUserId",
                table: "DebtAssignments",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_AspNetUsers_SelectedUserId",
                table: "DebtAssignments",
                column: "SelectedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_Debts_DebtId",
                table: "DebtAssignments",
                column: "DebtId",
                principalTable: "Debts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_NonUserDebtAssignment_NonUserDebtAssignmentId",
                table: "DebtAssignments",
                column: "NonUserDebtAssignmentId",
                principalTable: "NonUserDebtAssignment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_AspNetUsers_CreatorUserId",
                table: "DebtAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_AspNetUsers_SelectedUserId",
                table: "DebtAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_Debts_DebtId",
                table: "DebtAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_NonUserDebtAssignment_NonUserDebtAssignmentId",
                table: "DebtAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DebtAssignments",
                table: "DebtAssignments");

            migrationBuilder.DropColumn(
                name: "CurrencyPreference",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "DebtAssignments",
                newName: "DebtAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignments_SelectedUserId",
                table: "DebtAssignment",
                newName: "IX_DebtAssignment_SelectedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignments_NonUserDebtAssignmentId",
                table: "DebtAssignment",
                newName: "IX_DebtAssignment_NonUserDebtAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignments_DebtId",
                table: "DebtAssignment",
                newName: "IX_DebtAssignment_DebtId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtAssignments_CreatorUserId",
                table: "DebtAssignment",
                newName: "IX_DebtAssignment_CreatorUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DebtAssignment",
                table: "DebtAssignment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignment_AspNetUsers_CreatorUserId",
                table: "DebtAssignment",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignment_AspNetUsers_SelectedUserId",
                table: "DebtAssignment",
                column: "SelectedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignment_Debts_DebtId",
                table: "DebtAssignment",
                column: "DebtId",
                principalTable: "Debts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignment_NonUserDebtAssignment_NonUserDebtAssignmentId",
                table: "DebtAssignment",
                column: "NonUserDebtAssignmentId",
                principalTable: "NonUserDebtAssignment",
                principalColumn: "Id");
        }
    }
}
