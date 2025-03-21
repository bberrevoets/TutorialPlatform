using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorialPlatform.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToMarkdown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Chapters",
                newName: "MarkdownContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarkdownContent",
                table: "Chapters",
                newName: "Content");
        }
    }
}
