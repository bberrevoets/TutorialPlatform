using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Berrevoets.TutorialPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageToTutorial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Tutorials",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Tutorials");
        }
    }
}
