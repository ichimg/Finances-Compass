using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedNonUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_NonUserDebtAssignment_NonUserDebtAssignmentId",
                table: "DebtAssignments");

            migrationBuilder.DropTable(
                name: "NonUserDebtAssignment");

            migrationBuilder.CreateTable(
                name: "NonUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonUsers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_NonUsers_NonUserDebtAssignmentId",
                table: "DebtAssignments",
                column: "NonUserDebtAssignmentId",
                principalTable: "NonUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtAssignments_NonUsers_NonUserDebtAssignmentId",
                table: "DebtAssignments");

            migrationBuilder.DropTable(
                name: "NonUsers");

            migrationBuilder.CreateTable(
                name: "NonUserDebtAssignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonLastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonUserDebtAssignment", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DebtAssignments_NonUserDebtAssignment_NonUserDebtAssignmentId",
                table: "DebtAssignments",
                column: "NonUserDebtAssignmentId",
                principalTable: "NonUserDebtAssignment",
                principalColumn: "Id");
        }
    }
}
