using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorialPlatform.Migrations
{
    /// <inheritdoc />
    public partial class FixUserProgressForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTutorialProgresses_AspNetUsers_UserId1",
                table: "UserTutorialProgresses");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "UserTutorialProgresses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTutorialProgresses_UserId1",
                table: "UserTutorialProgresses",
                newName: "IX_UserTutorialProgresses_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTutorialProgresses_AspNetUsers_UserId",
                table: "UserTutorialProgresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTutorialProgresses_AspNetUsers_UserId",
                table: "UserTutorialProgresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserTutorialProgresses",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserTutorialProgresses_UserId",
                table: "UserTutorialProgresses",
                newName: "IX_UserTutorialProgresses_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTutorialProgresses_AspNetUsers_UserId1",
                table: "UserTutorialProgresses",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
