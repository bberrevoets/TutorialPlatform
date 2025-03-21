using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorialPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddTutorialCategoriesAndTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Tutorials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagTutorial",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    TutorialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTutorial", x => new { x.TagsId, x.TutorialsId });
                    table.ForeignKey(
                        name: "FK_TagTutorial_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagTutorial_Tutorials_TutorialsId",
                        column: x => x.TutorialsId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tutorials_CategoryId",
                table: "Tutorials",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TagTutorial_TutorialsId",
                table: "TagTutorial",
                column: "TutorialsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Categories_CategoryId",
                table: "Tutorials",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Categories_CategoryId",
                table: "Tutorials");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "TagTutorial");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tutorials_CategoryId",
                table: "Tutorials");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tutorials");
        }
    }
}
