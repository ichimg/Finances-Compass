using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNonUserDebtAssignmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonName",
                table: "NonUserDebtAssignment",
                newName: "PersonLastName");

            migrationBuilder.AddColumn<string>(
                name: "PersonFirstName",
                table: "NonUserDebtAssignment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonFirstName",
                table: "NonUserDebtAssignment");

            migrationBuilder.RenameColumn(
                name: "PersonLastName",
                table: "NonUserDebtAssignment",
                newName: "PersonName");
        }
    }
}
