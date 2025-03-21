using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorialPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProgressTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTutorialProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TutorialId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTutorialProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTutorialProgresses_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTutorialProgresses_Tutorials_TutorialId",
                        column: x => x.TutorialId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChapterProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserTutorialProgressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChapterProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChapterProgresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChapterProgresses_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChapterProgresses_UserTutorialProgresses_UserTutorialProgressId",
                        column: x => x.UserTutorialProgressId,
                        principalTable: "UserTutorialProgresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserChapterProgresses_ChapterId",
                table: "UserChapterProgresses",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapterProgresses_UserId",
                table: "UserChapterProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapterProgresses_UserTutorialProgressId",
                table: "UserChapterProgresses",
                column: "UserTutorialProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTutorialProgresses_TutorialId",
                table: "UserTutorialProgresses",
                column: "TutorialId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTutorialProgresses_UserId1",
                table: "UserTutorialProgresses",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChapterProgresses");

            migrationBuilder.DropTable(
                name: "UserTutorialProgresses");
        }
    }
}
