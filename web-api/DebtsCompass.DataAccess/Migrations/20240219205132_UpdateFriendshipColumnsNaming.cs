using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DebtsCompass.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFriendshipColumnsNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserOneId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_UserTwoId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "UserTwoId",
                table: "Friendships",
                newName: "SelectedUserId");

            migrationBuilder.RenameColumn(
                name: "UserOneId",
                table: "Friendships",
                newName: "RequesterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_UserTwoId",
                table: "Friendships",
                newName: "IX_Friendships_SelectedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_RequesterUserId",
                table: "Friendships",
                column: "RequesterUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_SelectedUserId",
                table: "Friendships",
                column: "SelectedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_RequesterUserId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_SelectedUserId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "SelectedUserId",
                table: "Friendships",
                newName: "UserTwoId");

            migrationBuilder.RenameColumn(
                name: "RequesterUserId",
                table: "Friendships",
                newName: "UserOneId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_SelectedUserId",
                table: "Friendships",
                newName: "IX_Friendships_UserTwoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserOneId",
                table: "Friendships",
                column: "UserOneId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_UserTwoId",
                table: "Friendships",
                column: "UserTwoId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
